using System.ComponentModel.DataAnnotations;

namespace Cards_Products_API.DTO_s
{
    public class CreateUserDTO
    {
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class UserResponseDTO
    {
        public int User_Id { get; set; }
        public string First_Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
