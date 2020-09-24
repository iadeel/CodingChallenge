using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customers.Entities;
using Customers.Services;

namespace Customers.Tests.Fakes
{
    public class CustomersRepositoryMoq : ICustomersRepository
    {
        private readonly List<Customer> _customers;

        public CustomersRepositoryMoq()
        {
            _customers = new List<Customer>
            {
                new Customer
                { Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                    FirstName = "James", LastName= "Smith", DateOfBirth = new DateTimeOffset(1990, 1, 1, 0, 0, 0, new TimeSpan(-2,0,0)) },
                new Customer
                { Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                    FirstName = "Jane", LastName= "Smith", DateOfBirth = new DateTimeOffset(1995, 1, 1, 0, 0, 0, new TimeSpan(-4,0,0)) }
            };
        }

        public async Task<Customer> GetCustomerAsync(Guid customerId)
        {
            await Task.Delay(10);
            return _customers.FirstOrDefault(c => c.Id == customerId);
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(string searchField)
        {
            if (string.IsNullOrWhiteSpace(searchField))
            {
                return await GetCustomersAsync();
            }

            var searchQuery = searchField.Trim();
            return _customers.Where(a => a.FirstName.Contains(searchQuery)
                                         || a.LastName.Contains(searchQuery)).ToList(); ;
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            await Task.Delay(10);
            return _customers;
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            await Task.Delay(10);
            var numOfCustomers = _customers.Count;
            _customers.Add(new Customer
                { Id = new Guid(),
                    FirstName = customer.FirstName, LastName= customer.LastName, DateOfBirth = customer.DateOfBirth });
            return _customers.Count > numOfCustomers;
        }

        public async Task<bool> DeleteCustomerAsync(Customer customer)
        {
            await Task.Delay(10);
            var numOfCustomers = _customers.Count;
            _customers.Remove(customer);
            return _customers.Count < numOfCustomers;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            await Task.Delay(10);
            return true;
        }

        public bool CustomerExists(Guid customerId)
        {
            return _customers.Any(c => c.Id == customerId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            await Task.Delay(10);
            return true;
        }
    }
}
