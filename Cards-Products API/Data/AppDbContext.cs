using Microsoft.EntityFrameworkCore;
using Cards_Products_API.Models;

namespace Cards_Products_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        // Cada DbSet representa una tabla
        public DbSet<Card> Cards { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar las relaciones entre las entidades
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.Card)
                .WithMany()
                .HasForeignKey(p => p.Card_Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Purchase>()
                .HasMany(p => p.PurchaseDetails)
                .WithOne(d => d.Purchase)
                .HasForeignKey(d => d.Purchase_Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
