using Cards_Products_API.DTO_s;
using Cards_Products_API.Interfaces;
using Cards_Products_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cards_Products_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetAll()
        {
            var cards = await _cardService.GetAllCards();
            return Ok(cards);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedCards([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _cardService.GetAllCardsPaged(page, pageSize);
            return Ok(result);
        }

        [HttpPost("increase-money")]
        public async Task<IActionResult> IncreaseCardMoney()
        {
            var updatedCards = await _cardService.IncreaseCardMoney();

            if (!updatedCards.Any())
                return NotFound("No hay tarjetas disponibles para actualizar.");

            return Ok(updatedCards);
        }

        [HttpPut("{cardId}")]
        public async Task<IActionResult> UpdateCard(int cardId, [FromBody] UpdateCardDTO dto)
        {
            var updatedCard = await _cardService.UpdateCard(cardId, dto);
            if (updatedCard == null) return NotFound($"Card with ID {cardId} not found.");
            return Ok(updatedCard);
        }
    }
}
