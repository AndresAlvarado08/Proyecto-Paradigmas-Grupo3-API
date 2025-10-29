using Cards_Products_API.Data;
using Cards_Products_API.DTO_s;
using Cards_Products_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards_Products_API.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Crear usuario
        public async Task<UserResponseDTO> CreateUser(CreateUserDTO dto)
        {
            var user = new User
            {
                First_Name = dto.First_Name,
                Last_Name = dto.Last_Name,
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password // Nota: asegúrate de hashear la contraseña
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponseDTO
            {
                User_Id = user.User_Id,
                First_Name = user.First_Name,
                Last_Name = user.Last_Name,
                Username = user.Username,
                Email = user.Email
            };
        }

        // Obtener todos los usuarios
        public async Task<IEnumerable<UserResponseDTO>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users.Select(u => new UserResponseDTO
            {
                User_Id = u.User_Id,
                First_Name = u.First_Name,
                Last_Name = u.Last_Name,
                Username = u.Username,
                Email = u.Email
            });
        }
    }
}
