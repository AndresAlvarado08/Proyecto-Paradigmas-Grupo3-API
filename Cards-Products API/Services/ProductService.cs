using Cards_Products_API.Interfaces;
using Cards_Products_API.Data;
using Cards_Products_API.Models;
using Microsoft.EntityFrameworkCore;
using Bogus;
using Cards_Products_API.DTO_s;

namespace Cards_Products_API.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> CreateRandomProducts()
        {
            var Products = new[] { "Laptop", "Smartphone", "Tablet", "Monitor", "Keyboard", "Mouse", "Printer", "Camera", "Headphones", "Speaker" };

            // Configurar Bogus para generar un producto
            var generator = new Faker<Product>()
                .RuleFor(p => p.Product_Name, f => f.PickRandom(Products))
                .RuleFor(p => p.Quantity, f => f.Random.Int(0, 80))
                .RuleFor(p => p.Price, f => f.Random.Int(200, 30000));

            var product = generator.Generate();

            // Guardar en la base de datos
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> UpdateProduct(int productId, UpdateProductDTO dto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return null;

            if (dto.Price.HasValue)
                product.Price = dto.Price.Value;

            if (dto.Quantity.HasValue)
                product.Quantity = dto.Quantity.Value;

            await _context.SaveChangesAsync();
            return product;
        }
    }
}
