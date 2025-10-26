using Cards_Products_API.Data;
using Cards_Products_API.DTO_s;
using Cards_Products_API.Interfaces;
using Cards_Products_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards_Products_API.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly AppDbContext _context;
        public PurchaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseDTO>> GetAllPurchases()
        {
            return await _context.Purchases
                .OrderByDescending(p => p.PurchaseDate)
                .Select(p => new PurchaseDTO
                {
                    Purchase_Id = p.Purchase_Id,
                    Total = p.Total,
                    PurchaseDate = p.PurchaseDate,
                    User_Id = p.Card.User_Id
                })
                .ToListAsync();
        }
    }
}
