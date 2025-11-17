using Cards_Products_API.DTO_s;
using Cards_Products_API.Models;

namespace Cards_Products_API.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<PaginacionDTO<Product>> GetAllProductsPaged(int page = 1, int pageSize = 10);
        Task<Product> CreateRandomProducts();
        Task<Product> UpdateProduct(int productId, UpdateProductDTO dto);
    }
}
