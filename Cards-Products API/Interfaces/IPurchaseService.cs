using Cards_Products_API.DTO_s;

namespace Cards_Products_API.Interfaces
{
    public interface IPurchaseService
    {
        Task<List<PurchaseDTO>> GetAllPurchases();
    }
}
