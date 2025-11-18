using System.ComponentModel.DataAnnotations;

namespace Cards_Products_API.DTO_s
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage = "El email del usuario es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres")]
        public string Password { get; set; } = string.Empty;
    }
}

