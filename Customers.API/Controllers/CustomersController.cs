using System;
using System.Threading.Tasks;
using AutoMapper;
using Customers.API.Filters;
using Customers.Services;
using Microsoft.AspNetCore.Mvc;

namespace Customers.API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersRepository _customersRepository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomersRepository customerRepository, IMapper mapper)
        {
            _customersRepository = customerRepository ??
                throw new ArgumentNullException(nameof(customerRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [CustomersResultFilter]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customersRepository.GetCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("searchField")]
        [CustomersResultFilter]
        public async Task<IActionResult> SearchCustomers(string searchField)
        {
            var customers = await _customersRepository.GetCustomersAsync(searchField);
            return Ok(customers);
        }


        [HttpGet("{id}", Name = "GetCustomer")]
        [CustomerResultFilter]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            var customer = await _customersRepository.GetCustomerAsync(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        [CustomerResultFilter]
        public async Task<IActionResult> AddCustomer(Models.Customer customerModel)
        {
            var customerEntity = _mapper.Map<Entities.Customer>(customerModel);

            await _customersRepository.AddCustomerAsync(customerEntity);

            return CreatedAtRoute("GetCustomer", new { id = customerEntity.Id }, customerEntity);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(Models.Customer customerModel)
        {
            if (!_customersRepository.CustomerExists(customerModel.Id))
            {
                return NotFound();
            }

            var customerEntity = _mapper.Map<Entities.Customer>(customerModel);

            await _customersRepository.UpdateCustomerAsync(customerEntity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customersRepository.GetCustomerAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            await _customersRepository.DeleteCustomerAsync(customer);

            return NoContent();
        }
    }
}
