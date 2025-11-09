using System.Threading.Tasks;

namespace Cards_Products_API.Messaging
{
    public interface IEventPublisher
    {
        Task PublishOrderCreatedAsync(object payload);
    }
}
