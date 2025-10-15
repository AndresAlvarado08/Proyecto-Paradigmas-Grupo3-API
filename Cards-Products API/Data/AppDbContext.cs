using Microsoft.EntityFrameworkCore;
using Cards_Products_API.Models;

namespace Cards_Products_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        // Cada DbSet representa una tabla
        public DbSet<Product> Products { get; set; }
    }
}
