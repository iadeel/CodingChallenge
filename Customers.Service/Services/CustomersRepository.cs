using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customers.Context;
using Customers.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customers.Services
{
    public class CustomersRepository : ICustomersRepository
    {
        private CustomersContext _context;
        public CustomersRepository(CustomersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Customer> GetCustomerAsync(Guid customerId)
        {
            if (customerId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(string searchField)
        {
            if (string.IsNullOrWhiteSpace(searchField))
            {
                return await GetCustomersAsync();
            }

            var collection = _context.Customers as IQueryable<Customer>;

            var searchQuery = searchField.Trim();
            collection = collection.Where(a => a.FirstName.Contains(searchQuery)
                || a.LastName.Contains(searchQuery));

            return await collection.ToListAsync();
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            customer.Id = Guid.NewGuid();

            await _context.Customers.AddAsync(customer);

            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            var entity = await GetCustomerAsync(customer.Id);
            entity.FirstName = customer.FirstName;
            entity.LastName = customer.LastName;
            entity.DateOfBirth = customer.DateOfBirth;
            return await SaveChangesAsync();
        }

        public bool CustomerExists(Guid customerId)
        {
            if (customerId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(customerId));
            }

            return _context.Customers.Any(c => c.Id == customerId);
        }

        public async Task<bool> DeleteCustomerAsync(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(customer));
            }

            _context.Customers.Remove(customer);

            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }

    }
}
