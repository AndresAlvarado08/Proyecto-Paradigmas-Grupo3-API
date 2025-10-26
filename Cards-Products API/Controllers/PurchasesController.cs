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
        public async Task<IActionResult> GetAll()
        {
            var purchases = await _purchaseService.GetAllPurchases();

            var result = purchases.Select(p => new
            {
                CardNumber = p.Card.Card_Number,
                CardUser = p.Card.User_Id,
                p.PurchaseDate
            });

            return Ok(result);
        }
    }
}
