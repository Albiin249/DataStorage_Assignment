using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using System.Linq.Expressions;

namespace Business.Services;

public class ProductService(ProductRepository repository) : IProductService
{
    private readonly ProductRepository _productRepository = repository;

    //CREATE
    public async Task CreateProductAsync(Product productModel)
    {
        var existingProduct = await _productRepository.GetAsync(x => x.ProductName == productModel.ProductName);
        if (existingProduct != null)
            throw new Exception("Product with this name already exists.");

        var productEntity = ProductFactory.Create(productModel);
        await _productRepository.CreateASync(productEntity!);
    }

    //READ
    public async Task<IEnumerable<Product?>> GetAllProductsAsync()
    {
        var productsEntities = await _productRepository.GetAllAsync();
        return productsEntities.Select(ProductFactory.Create);
    }

    public async Task<Product?> GetProductAsync(Expression<Func<ProductEntity, bool>> expression)
    {
        var productEntity = await _productRepository.GetAsync(expression);
        return ProductFactory.Create(productEntity);
    }

    //UPDATE
    public async Task<Product?> UpdateProductAsync(Product product)
    {
        var updatedEntity = await _productRepository.UpdateAsync(
            p => p.Id == product.Id,
            ProductFactory.Create(product)!
        );

        return updatedEntity != null ? ProductFactory.Create(updatedEntity) : null;
    }

    //DELETE
    public async Task<bool> DeleteProductAsync(int id)
    {
        var result = await _productRepository.DeleteAsync(x => x.Id == id);
        return result;
    }

    //ALREADY EXISTS
    public async Task<bool> CheckIfProductExists(Expression<Func<ProductEntity, bool>> expression)
    {
        return await _productRepository.AlreadyExistsAsync(expression);
    }

}
