using Cards_Products_API.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Cards_Products_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDTO model)
    {
        var client = _httpClientFactory.CreateClient();

        var tokenEndpoint = "http://26.9.80.46:8080/realms/Paradigmas/protocol/openid-connect/token";

        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", "payment-api"),
            new KeyValuePair<string, string>("client_secret", "9BvHuKBrIsBLRPwcSKkbXOD0x4LRiPt8"),
            new KeyValuePair<string, string>("username", model.Username ?? ""),
            new KeyValuePair<string, string>("password", model.Password ?? "")
        });

        var response = await client.PostAsync(tokenEndpoint, requestContent);

        if (!response.IsSuccessStatusCode)
            return BadRequest("Fallo en el Login, revisa tus credenciales");

        var accessToken = await response.Content.ReadAsStringAsync();
        return Ok(accessToken);
    }

    [HttpGet("secure")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult SecureTest()
    {
        return Ok("Token valido, puedes acceder a esta ruta");
    }
}
