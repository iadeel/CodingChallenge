using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Customers.Entities;

namespace Customers.Services
{
    public interface ICustomersRepository
    {
        Task<Customer> GetCustomerAsync(Guid customerId);
        Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<IEnumerable<Customer>> GetCustomersAsync(string searchField);
        Task<bool> AddCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(Customer customer);
        Task<bool> UpdateCustomerAsync(Customer customer);
        bool CustomerExists(Guid customerId);
        Task<bool> SaveChangesAsync();
    }
}
