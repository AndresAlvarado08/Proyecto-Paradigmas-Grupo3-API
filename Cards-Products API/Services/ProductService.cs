using Bogus;
using Cards_Products_API.Data;
using Cards_Products_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards_Products_API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> CreateRandomAsync();
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> CreateRandomAsync()
        {
            var Products = new[] { "Laptop", "Smartphone", "Tablet", "Monitor", "Keyboard", "Mouse", "Printer", "Camera", "Headphones", "Speaker" };

            // Configurar Bogus para generar un producto
            var faker = new Faker<Product>()
                .RuleFor(p => p.Product_Name, f => f.PickRandom(Products))
                .RuleFor(p => p.Quantity, f => f.Random.Int(0, 80))
                .RuleFor(p => p.Price, f => f.Random.Int(200, 30000));

            var product = faker.Generate();

            // Guardar en la base de datos
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
