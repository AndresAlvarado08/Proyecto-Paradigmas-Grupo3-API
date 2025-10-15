using Cards_Products_API.Models;
using Cards_Products_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cards_Products_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpPost("random")]
        public async Task<ActionResult<Product>> CreateRandom()
        {
            var product = await _productService.CreateRandomAsync();
            return CreatedAtAction(nameof(GetAll), new { id = product.Id }, product);
        }
    }
}
