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

        [HttpPost("random")]
        public async Task<ActionResult<Card>> CreateRandom()
        {
            var card = await _cardService.CreateRandomCard();
            return CreatedAtAction(nameof(GetAll), new { id = card.Id }, card);
        }
    }
}
