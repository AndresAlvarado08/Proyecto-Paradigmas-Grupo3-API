using Cards_Products_API.Data;
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

        public async Task<List<Purchase>> GetAllPurchases()
        {
            return await _context.Purchases
                .Include(p => p.Card.User_Id)   //Id del Usuario en la tarjeta
                .Include(p => p.Total)          //Total de la compra
                .OrderByDescending(p => p.PurchaseDate)
                .ToListAsync();
        }
    }
}
