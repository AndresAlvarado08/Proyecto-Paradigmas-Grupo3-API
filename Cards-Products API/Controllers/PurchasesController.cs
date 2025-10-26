using Cards_Products_API.Interfaces;
using Cards_Products_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cards_Products_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesController : Controller
    {
        private readonly IPurchaseService _purchaseService;

        public PurchasesController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPurchases()
        {
            var purchases = await _purchaseService.GetAllPurchases();
            return Ok(purchases); // aquí NO debes intentar acceder a .Card
        }
    }
}
