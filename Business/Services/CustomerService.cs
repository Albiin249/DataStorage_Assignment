using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using System.Linq.Expressions;

namespace Business.Services;

public class CustomerService(CustomerRepository customerRepository) : ICustomerService
{
    private readonly CustomerRepository _customerRepository = customerRepository;

    //CREATE
    public async Task CreateCustomerAsync(Customer customerModel)
    {
        var existingCustomer = await _customerRepository.GetAsync(x => x.CustomerName == customerModel.CustomerName);
        if (existingCustomer != null)
            throw new Exception("Customer with this name already exists.");

        var customerEntity = CustomerFactory.Create(customerModel);
        await _customerRepository.CreateASync(customerEntity!);
    }

    //READ
    public async Task<IEnumerable<Customer?>> GetAllCustomersAsync()
    {
        var customerEntities = await _customerRepository.GetAllAsync();
        return customerEntities.Select(CustomerFactory.Create);
    }

    public async Task<Customer?> GetCustomerAsync(Expression<Func<CustomerEntity, bool>> expression)
    {
        var customerEntity = await _customerRepository.GetAsync(expression);
        return CustomerFactory.Create(customerEntity);
    }

    //UPDATE
    public async Task<Customer?> UpdateCustomerAsync(Customer customer)
    {
        var updatedEntity = await _customerRepository.UpdateAsync(
            c => c.Id == customer.Id,
            CustomerFactory.Create(customer)!
        );

        return updatedEntity != null ? CustomerFactory.Create(updatedEntity) : null;
    }

    //DELETE
    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var result = await _customerRepository.DeleteAsync(x => x.Id == id);
        return result;
    }

    //ALREADY EXISTS
    public async Task<bool> CheckIfCustomerExistsAsync(Expression<Func<CustomerEntity, bool>> expression)
    {
        return await _customerRepository.AlreadyExistsAsync(expression);
    }

}
