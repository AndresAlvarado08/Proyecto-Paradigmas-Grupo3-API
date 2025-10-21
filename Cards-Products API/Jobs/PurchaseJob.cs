using Quartz;
using Cards_Products_API.Services;
using Cards_Products_API.Data;
using Cards_Products_API.Models;
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

            // Obtener todas las tarjetas y productos existentes
            var cards = _context.Cards.ToList();
            var products = _context.Products.ToList();

            if (!cards.Any() || !products.Any())
            {
                Console.WriteLine("No hay tarjetas o productos disponibles para realizar compras");
                return;
            }

            // Realizar 5 compras
            for (int i = 0; i < 5; i++)
            {
                var card = _faker.PickRandom(cards);
                var purchaseProducts = _faker.PickRandom(products, _faker.Random.Int(1, 3));

                foreach (var product in purchaseProducts)
                {
                    var purchase = new Purchase
                    {
                        CardId = card.Id,
                        ProductId = product.Id,
                        Amount = product.Price,
                        PurchaseDate = DateTime.UtcNow
                    };

                    card.Money -= product.Price;
                    _context.Purchases.Add(purchase);
                    Console.WriteLine($"Compra: Tarjeta {card.Card_Number}, Producto {product.Product_Name}, ${product.Price}");
                }
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("Finalizó PurchaseJob correctamente.\n\n\n");
        }
    }
}
