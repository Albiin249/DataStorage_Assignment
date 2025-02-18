using Business.Models;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> CheckIfCustomerExistsAsync(Expression<Func<CustomerEntity, bool>> expression);
        Task CreateCustomerAsync(Customer customerModel);
        Task<bool> DeleteCustomerAsync(int id);
        Task<IEnumerable<Customer?>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerAsync(Expression<Func<CustomerEntity, bool>> expression);
        Task<Customer?> UpdateCustomerAsync(Customer customer);
    }
}