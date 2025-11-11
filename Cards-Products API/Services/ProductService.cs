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
            var Products = new[]
            {
                "Laptop", "Smartphone", "Tablet", "Monitor",
                "Keyboard", "Mouse", "Printer", "Camera",
                "Headphones", "Speaker", "Smartwatch", "Router",
                "External Hard Drive", "USB Flash Drive", "Webcam",
                "Microphone", "Graphics Card", "Motherboard"
            };

            // Generar un nombre aleatorio
            var faker = new Faker();
            var randomName = faker.PickRandom(Products);

            // Buscar si ya existe un producto con ese nombre
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Product_Name == randomName);

            if (existingProduct != null)
            {
                // Si ya existe, aumentar su cantidad de forma aleatoria
                int addedQuantity = faker.Random.Int(10, 25);
                existingProduct.Quantity += addedQuantity;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Producto existente '{existingProduct.Product_Name}' actualizado. +{addedQuantity} unidades.");

                return existingProduct;
            }

            // Si no existe, crear un nuevo producto
            var newProduct = new Product
            {
                Product_Name = randomName,
                Quantity = faker.Random.Int(20, 50),
                Price = faker.Random.Int(15000, 50000)
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Nuevo producto creado: {newProduct.Product_Name}");

            return newProduct;
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
