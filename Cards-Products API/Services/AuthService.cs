using Cards_Products_API.Data;
using Cards_Products_API.DTO_s;
using Microsoft.EntityFrameworkCore;

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
            // Validar usuario en la base de datos
            //var user = await _context.Users
            //    .FirstOrDefaultAsync(u => u.Username == dto.Username && u.Password == dto.Password);

            //if (user == null)
            //    return null; // Usuario no encontrado o credenciales incorrectas

            // Si las credenciales son válidas, obtener el token de Keycloak
            var client = _httpClientFactory.CreateClient();

            var tokenEndpoint = "http://26.9.80.46:8080/realms/Paradigmas/protocol/openid-connect/token";

            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", "payment-api"),
                new KeyValuePair<string, string>("client_secret", "9BvHuKBrIsBLRPwcSKkbXOD0x4LRiPt8"),
                new KeyValuePair<string, string>("username", dto.Username ?? ""),
                new KeyValuePair<string, string>("password", dto.Password ?? "")
            });

            var response = await client.PostAsync(tokenEndpoint, requestContent);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error al solicitar el token en Keycloak.");

            return await response.Content.ReadAsStringAsync();
        }
    }
}
