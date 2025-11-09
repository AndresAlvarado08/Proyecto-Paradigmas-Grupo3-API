using Cards_Products_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards_Products_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        // Cada DbSet representa una tabla
        public DbSet<User> Users { get; set; }
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

            modelBuilder.Entity<Purchase>()
                .HasKey(p => p.Purchase_Id);

            modelBuilder.Entity<PurchaseDetail>()
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.Product_Id);

            modelBuilder.Entity<PurchaseDetail>()
                .HasKey(d => d.Purchase_Detail_Id);

            base.OnModelCreating(modelBuilder);
        }

        // Tablas
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Product ----------
            // Si tu Product.Price es decimal, fijamos precisión para evitar warnings/errores en MySQL/SQL Server.
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            // ---------- Order ----------
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderId)   // idempotencia de órdenes
                .IsUnique();

            modelBuilder.Entity<Order>()
                .Property(o => o.Total)
                .HasPrecision(18, 2);

            // ---------- OrderItem ----------
            modelBuilder.Entity<OrderItem>()
                .Property(i => i.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderIdFk)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
