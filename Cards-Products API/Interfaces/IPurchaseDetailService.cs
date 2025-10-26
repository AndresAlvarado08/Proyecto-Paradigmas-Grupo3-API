using Cards_Products_API.DTO_s;
using Cards_Products_API.Models;

namespace Cards_Products_API.Interfaces
{
    public interface IPurchaseDetailService
    {
        Task<List<PurchaseDetailDTO>> GetAllPurchaseDetails();
        Task<List<PurchaseDetail>> GetByPurchaseId(int purchaseId);
        Task<PurchaseDetail> CreateDetail(PurchaseDetail detail);
    }
}
