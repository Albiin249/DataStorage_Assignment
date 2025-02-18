using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class CustomerFactory
{

    //Från Model till Entity
    public static CustomerEntity? Create(Customer form) => form == null ? null : new()
    {
        Id = form.Id,
        CustomerName = form.CustomerName
    };

    //Från Entity till Model
    public static Customer? Create(CustomerEntity entity) => entity == null ? null : new()
    {
        Id = entity.Id,
        CustomerName = entity.CustomerName
    };

}
