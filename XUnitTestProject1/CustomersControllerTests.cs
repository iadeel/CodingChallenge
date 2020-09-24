using System;
using System.Collections.Generic;
using AutoMapper;
using Customers.API.Controllers;
using Customers.API.Profiles;
using Customers.Services;
using Customers.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Customers.Tests
{
    public class CustomersControllerTests
    {
        readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            var customersProfile = new CustomersProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(customersProfile));
            var mapper = new Mapper(configuration);
            ICustomersRepository service = new CustomersRepositoryMoq();
            _controller = new CustomersController(service, mapper);
        }

        [Fact]
        public void GetCustomersOkResultTest()
        {
            var okResult = _controller.GetCustomers();
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetCustomersReturnsAllCustomersTest()
        {
            var okResult = _controller.GetCustomers().Result as OkObjectResult;
            var customers = Assert.IsType<List<Entities.Customer>>(okResult?.Value);
            Assert.Equal(2, customers.Count);
        }

        [Fact]
        public void SearchCustomersReturnsAllMatchingCustomersTest()
        {
            var okResult = _controller.SearchCustomers("Jane").Result as OkObjectResult;
            var customers = Assert.IsType<List<Entities.Customer>>(okResult?.Value);
            Assert.Equal(1, customers.Count);
        }
        [Fact]
        public void SearchCustomersWithEmptySearchFieldReturnsAllCustomersTest()
        {
            var okResult = _controller.SearchCustomers("").Result as OkObjectResult;
            var customers = Assert.IsType<List<Entities.Customer>>(okResult?.Value);
            Assert.Equal(2, customers.Count);
        }

        [Fact]
        public void GetCustomerReturnsCustomerByIdTest()
        {
            var okResult = _controller.GetCustomer(new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200")).Result as OkObjectResult;
            var customer = Assert.IsType<Entities.Customer>(okResult?.Value);
            Assert.Equal("James", customer.FirstName);
            Assert.Equal("Smith", customer.LastName);
        }

        [Fact]
        public void GetCustomerNotFoundTest()
        {
            var notFoundResult = _controller.GetCustomer(new Guid("ab4bd817-98cd-4cf3-a80a-53ea0cd9c200")).Result as NotFoundResult;
            Assert.Equal(404, notFoundResult?.StatusCode);
        }

        [Fact]
        public void AddCustomerTest()
        {
            var customer = new Customers.API.Models.Customer
            {
                Id = new Guid(),
                FirstName = "Steve",
                LastName = "Smith",
                DateOfBirth = new DateTimeOffset(1990, 1, 1, 0, 0, 0, new TimeSpan(-2, 0, 0))
            };
            var createdAtRouteResult = _controller.AddCustomer(customer).Result as CreatedAtRouteResult;
            Assert.Equal(201, createdAtRouteResult?.StatusCode);
        }

        [Fact]
        public void UpdateCustomerTest()
        {
            var customer = new Customers.API.Models.Customer
            {
                Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                FirstName = "James",
                LastName = "Smith",
                DateOfBirth = new DateTimeOffset(1990, 1, 1, 0, 0, 0, new TimeSpan(-2, 0, 0))
            };
            var noContentResult = _controller.UpdateCustomer(customer).Result as NoContentResult;
            Assert.Equal(204, noContentResult?.StatusCode);
        }

        [Fact]
        public void UpdateCustomerNotFoundTest()
        {
            var customer = new Customers.API.Models.Customer
            {
                Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c201"),
                FirstName = "James",
                LastName = "Smith",
                DateOfBirth = new DateTimeOffset(1990, 1, 1, 0, 0, 0, new TimeSpan(-2, 0, 0))
            };
            var notFoundResult = _controller.UpdateCustomer(customer).Result as NotFoundResult;
            Assert.Equal(404, notFoundResult?.StatusCode);
        }

        [Fact]
        public void DeleteCustomerTest()
        {
            var noContentResult = _controller.DeleteCustomer(new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200")).Result as NoContentResult;
            Assert.Equal(204, noContentResult?.StatusCode);
        }

        [Fact]
        public void DeleteCustomerNotFoundTest()
        {
            var notFoundResult = _controller.DeleteCustomer(Guid.Empty).Result as NotFoundResult;
            Assert.Equal(404, notFoundResult?.StatusCode);
        }
    }
}
