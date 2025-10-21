using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using Cards_Products_API.Dtos;
using Cards_Products_API.Services;

namespace Cards_Products_API.Jobs
{
    public class PurchaseJob : IJob
    {
        private readonly IOrderService _orders;
        private readonly ILogger<PurchaseJob> _logger;

        public PurchaseJob(IOrderService orders, ILogger<PurchaseJob> logger)
        {
            _orders = orders;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var dto = new SimulateOrderDto { ItemsCount = 2 };
                var order = await _orders.SimulateOnceAsync(dto);
                _logger.LogInformation("Quartz → Simulated order {OrderId} total {Total}", order.OrderId, order.Total);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error running PurchaseJob");
            }
        }
    }
}
