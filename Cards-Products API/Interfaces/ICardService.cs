using Cards_Products_API.Models;
using Cards_Products_API.DTO_s;

namespace Cards_Products_API.Interfaces
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> GetAllCards();
        Task<PaginacionDTO<Card>> GetAllCardsPaged(int page, int pageSize);
        Task<Card> CreateRandomCard(double probabilityExpired = 0.35, bool formatWithSpaces = true);
        Task<List<UpdateMoneyCard>> IncreaseCardMoney();
        Task<Card?> UpdateCard(int cardId, UpdateCardDTO dto);
    }
}
