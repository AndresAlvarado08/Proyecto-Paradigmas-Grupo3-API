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
            // Configurar Bogus para generar un producto
            var faker = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => f.Random.Decimal(10, 500))
                .RuleFor(p => p.CreatedAt, f => DateTime.UtcNow);

            var product = faker.Generate();

            // Guardar en la base de datos
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
