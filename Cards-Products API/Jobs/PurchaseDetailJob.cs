using Cards_Products_API.Data;
using Cards_Products_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards_Products_API.Jobs
{
    public class PurchaseDetailJob
    {
        private readonly AppDbContext _context;
        private readonly Random _random = new Random();

        public PurchaseDetailJob(AppDbContext context)
        {
            _context = context;
        }

        public async Task Execute(int purchaseId)
        {
            Console.WriteLine("Ejecutando PurchaseDetailJob...");

            // Traer la compra y los productos disponibles
            var purchase = await _context.Purchases.FindAsync(purchaseId);
            var products = await _context.Products.ToListAsync();

            if (purchase == null || products.Count == 0)
            {
                Console.WriteLine("No se encontró la compra o no hay productos disponibles.");
                return;
            }

            // Cantidad de detalles aleatoria (1 a 3)
            int detailCount = _random.Next(1, 4);

            int total = 0;

            for (int i = 0; i < detailCount; i++)
            {
                var product = products[_random.Next(products.Count)];
                int quantity = _random.Next(1, 5); // 1 a 4 unidades
                int subTotal = (product.Price * quantity);

                var detail = new PurchaseDetail
                {
                    Purchase_Id = purchaseId,
                    Product_Id = product.Product_Id,
                    Quantity = quantity,
                    Total = subTotal
                };

                _context.PurchaseDetails.Add(detail);
                total += subTotal;
            }

            // Actualiza el subtotal de la compra
            purchase.SubTotal = total;
            await _context.SaveChangesAsync();
        }
    }
}