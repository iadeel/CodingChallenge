using Customers.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customers.Context
{
    public class CustomersContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomersContext(DbContextOptions<CustomersContext> options) : base(options)
        {

        }
    }
}
