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
            return await _context.PurchaseDetails
                .Select(d => new PurchaseDetailDTO
                {
                    Purchase_Detail_Id = d.Purchase_Detail_Id,
                    Purchase_Id = d.Purchase_Id,
                    Product_Id = d.Product_Id,
                    Quantity = d.Quantity,
                    SubTotal = d.SubTotal
                })
                .ToListAsync();
        }

        public async Task<List<PurchaseDetail>> GetByPurchaseId(int purchaseId)
        {
            return await _context.PurchaseDetails
                .Where(d => d.Purchase_Id == purchaseId)
                .Include(d => d.Product)
                .ToListAsync();
        }

        public async Task<PurchaseDetail> CreateDetail(PurchaseDetail detail)
        {
            _context.PurchaseDetails.Add(detail);
            await _context.SaveChangesAsync();
            return detail;
        }
    }
}
