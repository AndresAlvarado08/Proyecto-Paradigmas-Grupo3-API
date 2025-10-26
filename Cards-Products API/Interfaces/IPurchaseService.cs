using Cards_Products_API.Models;

namespace Cards_Products_API.Interfaces
{
    public interface IPurchaseService
    {
        Task<List<Purchase>> GetAllPurchases();
    }
}
