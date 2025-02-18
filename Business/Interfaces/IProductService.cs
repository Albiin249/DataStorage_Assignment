using Business.Models;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface IProductService
    {
        Task<bool> CheckIfProductExists(Expression<Func<ProductEntity, bool>> expression);
        Task CreateProductAsync(Product productModel);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<Product?>> GetAllProductsAsync();
        Task<Product?> GetProductAsync(Expression<Func<ProductEntity, bool>> expression);
        Task<Product?> UpdateProductAsync(Product product);
    }
}