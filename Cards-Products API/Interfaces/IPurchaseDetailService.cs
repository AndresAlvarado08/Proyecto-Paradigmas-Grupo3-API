using Cards_Products_API.DTO_s;
using Cards_Products_API.Models;

namespace Cards_Products_API.Interfaces
{
    public interface IPurchaseDetailService
    {
        Task<List<GetPurchaseDetailDTO>> GetAllPurchaseDetails();
        Task<List<GetPurchaseDetailDTO>> GetPurchaseDetailByPurchaseId(int purchase_Id);
        Task<PurchaseDetail> CreateDetail(PurchaseDetail detail);
    }
}
