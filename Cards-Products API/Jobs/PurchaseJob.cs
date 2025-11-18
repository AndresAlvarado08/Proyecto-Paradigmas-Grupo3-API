using Cards_Products_API.Data;
using Cards_Products_API.Models;
using Cards_Products_API.Services;
using System.Diagnostics;
using Quartz;
using Bogus;

namespace Cards_Products_API.Jobs
{
    public class PurchaseJob : IJob
    {
        private readonly AppDbContext _context;
        private readonly RabbitMQService _rabbitMQ;
        private readonly ILogger _logger;
        private readonly Faker _faker = new();

        public PurchaseJob(AppDbContext context, RabbitMQService rabbitMQ, ILogger<PurchaseJob> logger)
        {
            _context = context;
            _rabbitMQ = rabbitMQ;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Ejecutando PurchaseJob...");

            var cards = _context.Cards.ToList();
            var products = _context.Products.ToList();

            if (!cards.Any() || !products.Any())
            {
                _logger.LogWarning("No hay tarjetas o productos disponibles para realizar compras.");
                return;
            }

            var activity = new Activity("PurchaseJobExecution");
            activity.Start(); // ⬅️ Inicia telemetría

            int totalPurchases = 1; //_faker.Random.Int(1, 2);

            for (int i = 0; i < totalPurchases; i++)
            {
                var card = _faker.PickRandom(cards);
                var numProducts = 1; //_faker.Random.Int(1, 2);
                var selectedProducts = _faker.PickRandom(products, numProducts);

                // Validar stock ANTES de crear la compra
                foreach (var product in selectedProducts)
                {
                    int neededQty = _faker.Random.Int(1, 5);

                    if (product.Quantity < neededQty)
                    {
                        string msg =
                            $"COMPRA CANCELADA | Stock insuficiente para Product_Id = {product.Product_Id} ({product.Product_Name}). " +
                            $"Disponible = {product.Quantity}, Requerido={neededQty}\n";

                        Console.WriteLine(msg);
                        _logger.LogWarning(msg);

                        // Registrar evento en OpenTelemetry
                        activity?.AddEvent(new ActivityEvent("StockInsuficiente", tags: new ActivityTagsCollection
                        {
                            { "product_id", product.Product_Id },
                            { "product_name", product.Product_Name },
                            { "available_stock", product.Quantity },
                            { "requested_quantity", neededQty },
                            { "message", "Compra cancelada por stock insuficiente" }
                        }));

                        // NO publicar a RabbitMQ — simplemente continuar con siguiente compra
                        goto CompraCancelada;
                    }
                }

                // Si hay suficiente stock para todos → crear la compra
                int subtotal = selectedProducts.Sum(p => p.Price);

                var purchase = new Purchase
                {
                    Card_Id = card.Card_Id,
                    User_Id = card.User_Id,
                    SubTotal = subtotal,
                    Purchase_Date = DateOnly.FromDateTime(DateTime.Now)
                };

                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Compra creada Purchase_Id = {purchase.Purchase_Id}\n\n");

                // Crear detalles y restar stock
                foreach (var product in selectedProducts)
                {
                    int quantity = _faker.Random.Int(5, 10);

                    var detail = new PurchaseDetail
                    {
                        Purchase_Id = purchase.Purchase_Id,
                        Product_Id = product.Product_Id,
                        Quantity = quantity,
                        Total = product.Price * quantity
                    };

                    _context.PurchaseDetails.Add(detail);

                    // Restar stock
                    product.Quantity -= quantity;

                    _logger.LogInformation(
                        $"Detalle creado: {product.Product_Name}, Cantidad = {quantity}, NuevoStock = {product.Quantity}\n\n");
                }

                await _context.SaveChangesAsync();

                // ENVIAR A RABBIT SOLO SI TODO FUE EXITOSO
                try
                {
                    var mensajeRabbit = new
                    {
                        purchase_Id = purchase.Purchase_Id,
                        card_Id = purchase.Card_Id,
                        total = purchase.SubTotal,
                        purchaseDate = purchase.Purchase_Date,
                        user_Id = purchase.User_Id
                    };

                    _rabbitMQ.PublicarCompra(mensajeRabbit);
                    _logger.LogInformation($"Compra publicada en RabbitMQ ID = {purchase.Purchase_Id}\n");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error enviando a RabbitMQ: {ex.Message}\n");
                }

                continue;

            // Etiqueta para saltar cuando una compra se cancela
            CompraCancelada:
                continue;
            }

            _logger.LogInformation("PurchaseJob Finalizado con exito\n");
            activity?.Stop();
        }
    }
}