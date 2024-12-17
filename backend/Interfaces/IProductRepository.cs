using backend.DTOs;

namespace backend.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<ProductResponse>> GetAllProductAsync();
    Task<ProductResponse?> GetProductByIdAsync(int id);
    Task<int?> AddProductAsync(ProductCreate product);
    Task UpdateProductAsync(int id, ProductUpdate product);
    Task DeleteProductAsync(int id);
}
