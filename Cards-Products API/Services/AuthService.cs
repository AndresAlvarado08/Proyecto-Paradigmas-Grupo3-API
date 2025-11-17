using Cards_Products_API.Data;
using Cards_Products_API.DTO_s;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cards_Products_API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string?> LoginAsync(LoginUserDTO dto)
        {
            var client = _httpClientFactory.CreateClient();

            var tokenEndpoint = "http://26.9.80.46:8080/realms/Paradigmas/protocol/openid-connect/token";

            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", "payment-api"),
                new KeyValuePair<string, string>("client_secret", "9BvHuKBrIsBLRPwcSKkbXOD0x4LRiPt8"),
                new KeyValuePair<string, string>("username", dto.Email ?? ""),
                new KeyValuePair<string, string>("password", dto.Password ?? "")
            });

            var response = await client.PostAsync(tokenEndpoint, requestContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error al solicitar el token en Keycloak.");

            var json = await response.Content.ReadAsStringAsync();

            // Deserializar la respuesta
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);

            return tokenResponse?.AccessToken;
        }

        // Clase para mapear solo lo necesario
        public class TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string? AccessToken { get; set; }
        }
    }
}
