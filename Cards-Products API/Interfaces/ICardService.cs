using Cards_Products_API.Models;
using Cards_Products_API.DTO_s;

namespace Cards_Products_API.Interfaces
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> GetAllCards();
        Task<Card> CreateRandomCard(double probabilityExpired = 0.35, bool formatWithSpaces = true);
        Task<Card?> UpdateCard(int cardId, UpdateCardDTO dto);
    }
}
