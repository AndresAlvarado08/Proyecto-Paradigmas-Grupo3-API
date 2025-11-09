using Cards_Products_API.Data;
using Cards_Products_API.DTO_s;
using Cards_Products_API.Interfaces;
using Cards_Products_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards_Products_API.Services
{
    public class PurchaseDetailService : IPurchaseDetailService
    {
        private readonly AppDbContext _context;

        public PurchaseDetailService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseDetailDTO>> GetAllPurchaseDetails()
        {
            var purchases = await _context.Purchases
                .Include(p => p.Card)
                .Include(p => p.PurchaseDetails)
                    .ThenInclude(pd => pd.Product)
                .ToListAsync();

            if (!purchases.Any())
                return new List<PurchaseDetailDTO>();

            var result = new List<PurchaseDetailDTO>();

            foreach (var purchase in purchases)
            {
                foreach (var detail in purchase.PurchaseDetails)
                {
                    result.Add(new PurchaseDetailDTO
                    {
                        Purchase_Detail_Id = detail.Purchase_Detail_Id,
                        Purchase_Date = purchase.Purchase_Date,
                        User_Id = purchase.User_Id,
                        Detalle_Compra = new List<DetalleCompraDTO>
                        {
                            new DetalleCompraDTO
                            {
                                Purchase_Id = purchase.Purchase_Id,
                                Card_Id = purchase.Card_Id,
                                Product_Id = detail.Product_Id,
                                Product_Price = detail.Product.Price,
                                Quantity = detail.Quantity,
                                Total = detail.Total
                            }
                        }
                    });
                }
            }

            return result;
        }

        public async Task<List<PurchaseDetailDTO>> GetPurchaseDetailByPurchaseId(int purchase_Id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.Card)
                .Include(p => p.PurchaseDetails)
                    .ThenInclude(pd => pd.Product)
                .FirstOrDefaultAsync(p => p.Purchase_Id == purchase_Id);

            if (purchase == null)
                return new List<PurchaseDetailDTO>();

            return purchase.PurchaseDetails.Select(d => new PurchaseDetailDTO
            {
                Purchase_Detail_Id = d.Purchase_Detail_Id,
                Purchase_Date = purchase.Purchase_Date,
                User_Id = purchase.User_Id,
                Detalle_Compra = new List<DetalleCompraDTO>
                {
                    new DetalleCompraDTO
                    {
                        Purchase_Id = purchase.Purchase_Id,
                        Card_Id = purchase.Card_Id,
                        Product_Id = d.Product_Id,
                        Product_Price = d.Product.Price,
                        Quantity = d.Quantity,
                        Total = d.Total
                    }
                }
            }).ToList();
        }

        public async Task<PurchaseDetail> CreateDetail(PurchaseDetail detail)
        {
            _context.PurchaseDetails.Add(detail);
            await _context.SaveChangesAsync();
            return detail;
        }
    }
}
