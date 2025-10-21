using Cards_Products_API.Models;

namespace Cards_Products_API.Interfaces
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> GetAllCards();
        Task<Card> CreateRandomCard(double probabilityExpired = 0.35, bool formatWithSpaces = true);
    }
}
