using CustomerAPI.Controllers;
using FluentAssertions;
using CustomerAPI.Models;
using CustomerRepository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using CustomerDomainModel.Models;
using System.Collections.Generic;
namespace CustomerTests
{
    public class PolicyReferenceControllerTests
    {
        private readonly CustomerController _controller;
        private readonly Mock<ICustomerRepository> _customerRepository;

        public PolicyReferenceControllerTests()
        {
            _customerRepository = new Mock<ICustomerRepository>();
            _controller = new CustomerController(_customerRepository.Object);
        }

        [Fact]
        public async void GetCustomerById_returns_customer_when_data_exists()
        {
            // Arrange.
            long customerIdToGet = 1;
            var customerOne = new CustomerDetail
            {
                CustomerId = 1,
                FullName = "uma",
                Address = "indwin rejoice",
            };

            _customerRepository.Setup(x => x.GetCustomerByIdAsync(It.IsAny<long>())).ReturnsAsync(customerOne);
            var result = await _controller.GetCustomerByIdAsync(customerIdToGet);
            // Assert.
            Assert.Equal(200, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetCustomerById_returns_notfound_when_data_doesnot_exists()
        {
            // Arrange.
            long customerIdToGet = 999;
            var customerOne = new CustomerDetail
            {
                CustomerId = 1,
                FullName = "uma",
                Address = "indwin rejoice",
            };

            _customerRepository.Setup(x => x.GetCustomerByIdAsync(It.IsAny<long>())).ReturnsAsync(() => null);
            var result = await _controller.GetCustomerByIdAsync(customerIdToGet);
            // Assert.
            Assert.Equal(422, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void GetCustomers_returns_customers_when_data_exists()
        {
            // Arrange.
            var customerOne = new CustomerDetail
            {
                CustomerId = 1,
                FullName = "uma",
                Address = "indwin rejoice",
            };
            var customerTwo = new CustomerDetail
            {
                CustomerId = 2,
                FullName = "mahesh",
                Address = "indwin",
                IsActive = false,
            };
            var customerList = new List<CustomerDetail>();
            customerList.Add(customerOne);
            customerList.Add(customerTwo);
            var customerResponse = new GetCustomersResponse() { Customers = customerList };

            _customerRepository.Setup(x => x.GetRegisteredCustomersAsync()).ReturnsAsync(customerResponse);
            var result = await _controller.GetRegisteredCustomersAsync();
            // Assert.
            Assert.Equal(200, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void RegisterCustomer_returns_true_for_new_customer()
        {
            // Arrange.
            var customerOne = new CustomerRequest
            {
                FullName = "new customer",
                Address = "new address",
            };
            _customerRepository.Setup(x => x.RegisterCustomerAsync(It.IsAny<CustomerRequest>())).ReturnsAsync(true);
            var result = await _controller.RegisterCustomerAsync(customerOne);
            // Assert.
            Assert.Equal(201, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void RegisterCustomer_returns_false_for_existing_customer()
        {
            // Arrange.
            var customerOne = new CustomerRequest
            {
                FullName = "uma",
                Address = "indwin",
            };
            _customerRepository.Setup(x => x.RegisterCustomerAsync(It.IsAny<CustomerRequest>())).ReturnsAsync(false);
            var result = await _controller.RegisterCustomerAsync(customerOne);
            // Assert.
            Assert.Equal(422, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void UpdateCustomer_returns_true_for_existing_customer()
        {
            // Arrange.
            var customerOne = new UpdateCustomerRequest
            {
                FullName = "uma",
                Address = "indwin rejoice",
            };
            long customerIdToUpdate = 1;
            _customerRepository.Setup(x => x.UpdateCustomerAsync(It.IsAny<long>(), It.IsAny<UpdateCustomerRequest>())).ReturnsAsync(true);
            var result = await _controller.UpdateCustomerAsync(customerIdToUpdate, customerOne);
            // Assert.
            Assert.Equal(201, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void UpdateCustomer_returns_false_for_no_customer()
        {
            // Arrange.
            var customerOne = new UpdateCustomerRequest
            {
                FullName = "ninety nine",
                Address = "indwin rejoice",
            };
            long customerIdToUpdate = 99;
            _customerRepository.Setup(x => x.UpdateCustomerAsync(It.IsAny<long>(), It.IsAny<UpdateCustomerRequest>())).ReturnsAsync(false);
            var result = await _controller.UpdateCustomerAsync(customerIdToUpdate, customerOne);
            // Assert.
            Assert.Equal(422, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void DeleteCustomer_returns_true_for_existing_customer()
        {
            // Arrange.
            long customerIdToDelete = 9;
            _customerRepository.Setup(x => x.DeleteCustomerAsync(It.IsAny<long>())).ReturnsAsync(true);
            var result = await _controller.DeleteCustomerAsync(customerIdToDelete);
            // Assert.
            Assert.Equal(201, ((ObjectResult)result).StatusCode);
        }

        [Fact]
        public async void DeleteCustomer_returns_false_for_no_customer()
        {
            // Arrange.
            long customerIdToDelete = 99;
            _customerRepository.Setup(x => x.DeleteCustomerAsync(It.IsAny<long>())).ReturnsAsync(false);
            var result = await _controller.DeleteCustomerAsync(customerIdToDelete);
            // Assert.
            Assert.Equal(422, ((ObjectResult)result).StatusCode);
        }
    }
}