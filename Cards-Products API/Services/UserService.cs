using Bogus;
using Cards_Products_API.Data;
using Cards_Products_API.DTO_s;
using Cards_Products_API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cards_Products_API.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users.Select(u => new UserResponseDTO
            {
                User_Id = u.User_Id,
                First_Name = u.First_Name,
                Last_Name = u.Last_Name,
                Email = u.Email
            });
        }

        public async Task<PaginacionDTO<User>> GetPagedUsers(int page, int pageSize)
        {
            try
            {
                // Total de registros en la tabla Users
                var totalRecords = await _context.Users.CountAsync();

                // Total de páginas
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                // Datos paginados
                var users = await _context.Users
                    .OrderBy(c => c.User_Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginacionDTO<User>
                {
                    Total_Records = totalRecords,
                    Total_Pages = totalPages,
                    Page = page,
                    Page_Size = pageSize,
                    Items = users
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Crear usuario
        public async Task<User> CreateRandomUser()
        {
            var FirstNames = new[] { "John", "Jane", "Michael", "Emily", "David", "Sarah", "Robert", "Olivia", "James", "Sophia" };
            var LastNames = new[] { "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin" };

            var _random = new Random();
            var first = FirstNames[_random.Next(FirstNames.Length)];
            var last = LastNames[_random.Next(LastNames.Length)];
            var email = $"{first.ToLower()}.{last.ToLower()}{_random.Next(100, 150)}@gmail.com";

            var user = new User
            {
                First_Name = first,
                Last_Name = last,
                Email = email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
