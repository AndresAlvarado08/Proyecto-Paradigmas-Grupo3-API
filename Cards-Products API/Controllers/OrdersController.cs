using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Cards_Products_API.Dtos;
using Cards_Products_API.Services;

namespace Cards_Products_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _svc;

        public OrdersController(IOrderService svc) => _svc = svc;

        [HttpPost("simulate")]
        public async Task<IActionResult> Simulate([FromBody] SimulateOrderDto dto)
        {
            var order = await _svc.SimulateOnceAsync(dto);
            return Ok(new
            {
                orderId = order.OrderId,
                total = order.Total,
                currency = order.Currency,
                status = order.Status.ToString().ToUpperInvariant(),
                items = order.Items.Select(i => new { i.ProductId, i.Qty, i.UnitPrice }),
                traceId = order.TraceId
            });
        }
    }
}
