using Cards_Products_API.Data;
using Cards_Products_API.Models;
using Cards_Products_API.Services;
using Quartz;
using Bogus;

namespace Cards_Products_API.Jobs
{
    public class PurchaseJob : IJob
    {
        private readonly AppDbContext _context;
        private readonly RabbitMQService _rabbitMQ;
        private readonly Faker _faker = new();

        public PurchaseJob(AppDbContext context, RabbitMQService rabbitMQ)
        {
            _context = context;
            _rabbitMQ = rabbitMQ;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Ejecutando PurchaseJob...");

            var cards = _context.Cards.ToList();
            var products = _context.Products.ToList();

            if (!cards.Any() || !products.Any())
            {
                Console.WriteLine("No hay tarjetas o productos disponibles para realizar compras.");
                return;
            }

            // Generar entre 1 y 5 compras
            int totalPurchases = _faker.Random.Int(1, 2);

            for (int i = 0; i < totalPurchases; i++)
            {
                var card = _faker.PickRandom(cards);
                var numProducts = _faker.Random.Int(1, 2);
                var selectedProducts = _faker.PickRandom(products, numProducts);

                // Calcular subtotal de la compra
                int subtotal = selectedProducts.Sum(p => p.Price);

                // Crear la compra en la base de datos
                var purchase = new Purchase
                {
                    Card_Id = card.Card_Id,
                    User_Id = card.User_Id,
                    SubTotal = subtotal,
                    Purchase_Date = DateOnly.FromDateTime(DateTime.Now)
                };

                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Compra creada en BD: Purchase_Id={purchase.Purchase_Id}, SubTotal=${subtotal}");

                // Crear detalles para cada producto comprado
                foreach (var product in selectedProducts)
                {
                    int quantity = _faker.Random.Int(1, 5);

                    var detail = new PurchaseDetail
                    {
                        Purchase_Id = purchase.Purchase_Id,
                        Product_Id = product.Product_Id,
                        Quantity = quantity,
                        Total = product.Price * quantity
                    };

                    _context.PurchaseDetails.Add(detail);
                    Console.WriteLine($"Detalle: Producto={product.Product_Name}, Cantidad={quantity}, Subtotal=${detail.Total}");
                }

                await _context.SaveChangesAsync();
                Console.WriteLine($"Detalles guardados para Purchase_Id={purchase.Purchase_Id}");

                // ⭐ PUBLICAR A RABBITMQ
                // Crear objeto con el formato esperado por el Grupo 4
                var compraParaRabbit = new
                {
                    purchase_Id = purchase.Purchase_Id,
                    card_Id = purchase.Card_Id,
                    total = purchase.SubTotal,
                    purchaseDate = purchase.Purchase_Date,
                    user_Id = card.User_Id  // Asumiendo que Card tiene User_Id
                };

                try
                {
                    _rabbitMQ.PublicarCompra(compraParaRabbit);
                    Console.WriteLine($"Compra publicada a RabbitMQ: Purchase_Id={purchase.Purchase_Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error publicando a RabbitMQ: {ex.Message}");
                    // No lanzar excepción para no detener el proceso
                }

                Console.WriteLine();
            }

            Console.WriteLine($"PurchaseJob finalizado correctamente. Total de compras creadas: {totalPurchases}\n");
        }
    }
}