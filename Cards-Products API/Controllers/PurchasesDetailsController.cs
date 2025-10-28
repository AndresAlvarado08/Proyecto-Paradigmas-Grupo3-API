using Cards_Products_API.DTO_s;
using Cards_Products_API.Interfaces;
using Cards_Products_API.Models;
using Cards_Products_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cards_Products_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseDetailsController : ControllerBase
    {
        private readonly IPurchaseDetailService _service;

        public PurchaseDetailsController(IPurchaseDetailService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDetailDTO>>> GetPurchaseDetails()
        {
            var details = await _service.GetAllPurchaseDetails();
            return Ok(details);
        }

        [HttpGet("purchase/{purchaseId}")]
        public async Task<IActionResult> GetByPurchase(int purchaseId)
        {
            var details = await _service.GetByPurchaseId(purchaseId);
            return Ok(details);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PurchaseDetail detail)
        {
            var created = await _service.CreateDetail(detail);
            return CreatedAtAction(nameof(GetByPurchase), new { purchaseId = detail.Purchase_Id }, created);
        }
    }
}
