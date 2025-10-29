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

        [HttpGet("purchaseDetails/{purchaseId}")]
        public async Task<IActionResult> GetDetailsByPurchaseId(int purchaseId)
        {
            var details = await _service.GetPurchaseDetailByPurchaseId(purchaseId);
            if (details == null || !details.Any()) return NotFound();
            return Ok(details);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PurchaseDetail detail)
        {
            var created = await _service.CreateDetail(detail);
            return CreatedAtAction(nameof(GetDetailsByPurchaseId), new { purchaseId = detail.Purchase_Id }, created);
        }
    }
}
