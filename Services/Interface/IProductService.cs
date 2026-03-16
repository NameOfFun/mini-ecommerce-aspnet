using Mini_E_Commerce.Dtos.Product;
using Mini_E_Commerce.Models;

namespace Mini_E_Commerce.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto?> GetProductById(int id);
        Task<ProductDto> CreateProduct(ProductDto product);
        Task<bool> UpdateProduct(int id, ProductDto product);
        Task<bool> DeleteProduct(int id);
    }
}
