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
        public DbSet<Card> Cards { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurar las relaciones entre las entidades
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Card)
                .WithMany()
                .HasForeignKey(p => p.CardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
