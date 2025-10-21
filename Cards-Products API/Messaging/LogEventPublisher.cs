using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cards_Products_API.Messaging
{
    public class LogEventPublisher : IEventPublisher
    {
        private readonly ILogger<LogEventPublisher> _logger;
        public LogEventPublisher(ILogger<LogEventPublisher> logger) => _logger = logger;

        public Task PublishOrderCreatedAsync(object payload)
        {
            _logger.LogInformation("order.created → {json}", JsonSerializer.Serialize(payload));
            return Task.CompletedTask;
        }
    }
}
