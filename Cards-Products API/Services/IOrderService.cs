using System.Threading.Tasks;
using Cards_Products_API.Dtos;
using Cards_Products_API.Models;

namespace Cards_Products_API.Services
{
    public interface IOrderService
    {
        Task<Order> SimulateOnceAsync(SimulateOrderDto input, string? externalTraceId = null);
    }
}
