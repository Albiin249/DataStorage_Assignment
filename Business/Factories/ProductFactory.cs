using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ProductFactory
{
    //Från ProductModel till Entity
    public static ProductEntity? Create(Product model) => model == null ? null : new()
    {
        Id = model.Id,
        ProductName = model.ProductName,
        Price = model.Price
    };

    //Från ProductEntity till Model
    public static Product? Create(ProductEntity entity) => entity == null ? null : new()
    {
        Id = entity.Id,
        ProductName = entity.ProductName,
        Price = entity.Price
    };
}
