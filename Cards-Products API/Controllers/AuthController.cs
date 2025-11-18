using Cards_Products_API.DTO_s;
using Cards_Products_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cards_Products_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO dto)
        {
            var token = await _authService.LoginAsync(dto);

            if (token == null)
                return BadRequest("Email o contraseña incorrectos.");

            return Ok(token);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok(new { message = "Sesión cerrada correctamente" });
        }

        [HttpGet("secure")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult SecureTest()
        {
            return Ok("Token válido, puedes acceder a esta ruta.");
        }
    }
}
