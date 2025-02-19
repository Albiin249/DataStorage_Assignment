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
        await _customerRepository.BeginTransactionAsync();

        try
        {
            var existingCustomer = await _customerRepository.GetAsync(x => x.CustomerName == customerModel.CustomerName);
            if (existingCustomer != null)
                throw new Exception("Customer with this name already exists.");

            var customerEntity = CustomerFactory.Create(customerModel);
            await _customerRepository.CreateASync(customerEntity!);
            await _customerRepository.CommitTransactionAsync();
        }
        catch
        {
            await _customerRepository.RollBackTransactionAsync();
        }
        
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
        await _customerRepository.BeginTransactionAsync();

        try
        {
            var updatedEntity = await _customerRepository.UpdateAsync(
                c => c.Id == customer.Id,
                CustomerFactory.Create(customer)!
            );
            if (updatedEntity == null)
                return null;
            await _customerRepository.CommitTransactionAsync();
            return CustomerFactory.Create(updatedEntity);
            
        }
        catch
        {
            await _customerRepository.RollBackTransactionAsync();
            throw;
        }

    }

    //DELETE
    public async Task<bool> DeleteCustomerAsync(int id)
    {
        await _customerRepository.BeginTransactionAsync();
        try
        {
            var result = await _customerRepository.DeleteAsync(x => x.Id == id);
            if(!result) 
                return false;

            await _customerRepository.CommitTransactionAsync();
            return result;
        }
        catch
        {
            await _customerRepository.RollBackTransactionAsync();
            throw;
        }
        
    }

    //ALREADY EXISTS
    public async Task<bool> CheckIfCustomerExistsAsync(Expression<Func<CustomerEntity, bool>> expression)
    {
        return await _customerRepository.AlreadyExistsAsync(expression);
    }

}
