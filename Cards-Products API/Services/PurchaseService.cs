using Cards_Products_API.Data;
using Cards_Products_API.DTO_s;
using Cards_Products_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cards_Products_API.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PurchaseService> _logger;

        public PurchaseService(AppDbContext context, ILogger<PurchaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<PurchaseDTO>> GetAllPurchases()
        {
            return await _context.Purchases
                .Select(p => new PurchaseDTO
                {
                    Purchase_Id = p.Purchase_Id,
                    Card_Id = p.Card_Id,
                    SubTotal = p.SubTotal,
                    Purchase_Date = p.Purchase_Date,
                    User_Id = p.Card.User_Id
                })
                .ToListAsync();
        }
    }
}