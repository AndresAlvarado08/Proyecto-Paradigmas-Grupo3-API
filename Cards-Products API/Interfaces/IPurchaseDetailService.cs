using Cards_Products_API.DTO_s;
using Cards_Products_API.Models;

namespace Cards_Products_API.Interfaces
{
    public interface IPurchaseDetailService
    {
        Task<List<PurchaseDetailDTO>> GetAllPurchaseDetails();
        Task<List<PurchaseDetailDTO>> GetPurchaseDetailByPurchaseId(int purchase_Id);
        Task<PurchaseDetail> CreateDetail(PurchaseDetail detail);
    }
}
