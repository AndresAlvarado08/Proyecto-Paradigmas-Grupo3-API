using Cards_Products_API.Models;

namespace Cards_Products_API.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> CreateRandomProducts();
    }
}
