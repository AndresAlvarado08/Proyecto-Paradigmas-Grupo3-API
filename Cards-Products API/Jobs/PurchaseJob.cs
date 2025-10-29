using Cards_Products_API.Data;
using Cards_Products_API.Models;
using Quartz;
using Bogus;

namespace Cards_Products_API.Jobs
{
    public class PurchaseJob : IJob
    {
        private readonly AppDbContext _context;
        private readonly Faker _faker = new();

        public PurchaseJob(AppDbContext context)
        {
            _context = context;
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

            // Generar entre 3 y 5 compras
            int totalPurchases = _faker.Random.Int(3, 5);

            for (int i = 0; i < totalPurchases; i++)
            {
                var card = _faker.PickRandom(cards);
                var numProducts = _faker.Random.Int(1, 3);
                var selectedProducts = _faker.PickRandom(products, numProducts);

                // Calcular subtotal de la compra
                int subtotal = selectedProducts.Sum(p => p.Price);

                // Crear la compra
                var purchase = new Purchase
                {
                    Card_Id = card.Card_Id,
                    SubTotal = subtotal,
                    Purchase_Date = DateTime.UtcNow
                };

                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Compra creada: Purchase_Id={purchase.Purchase_Id}, SubTotal=${subtotal}");

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
                Console.WriteLine($"Detalles generados para Purchase_Id={purchase.Purchase_Id}\n");
            }

            Console.WriteLine("Finalizó PurchaseJob correctamente.\n\n");
        }
    }
}
