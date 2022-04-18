using System;
using CustomerRepository.Models;
using CustomerRepository;
using CustomerDomainModel.Models;
using AutoMapper;
using Moq;
using Moq.Protected;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
namespace CustomerTests
{
    public class CustomerRepoTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly CustomerRepo _customerRepo;
        UserDbContext dbContextTest = null;
        public CustomerRepoTests()
        {
            _mapper = new Mock<IMapper>();
            var customerOne = new Customer
            {
                CustomerId = 1,
                FullName = "uma",
                Address = "indwin rejoice",
                IsActive = true,
            };

            var customerTwo = new Customer
            {
                CustomerId = 2,
                FullName = "mahesh",
                Address = "indwin",
                IsActive = false,
            };

            var customerToDelete = new Customer
            {
                CustomerId = 9,
                FullName = "to delete",
                Address = "forest",
                IsActive = true,
            };

            var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            dbContextTest = new UserDbContext(options);
            dbContextTest.AddRange(customerOne);
            dbContextTest.AddRange(customerTwo);
            dbContextTest.AddRange(customerToDelete);
            dbContextTest.SaveChanges();

            _customerRepo = new CustomerRepo(dbContextTest, _mapper.Object);
        }

        [Fact]
        public async void GetCustomerById_returns_customer_when_data_exists()
        {
            var customerOne = new CustomerDetail
            {
                CustomerId = 1,
                FullName = "uma",
                Address = "indwin rejoice",
            };

            // Arrange.
            long customerIdToGet = 1;
            this._mapper.Setup(x => x.Map<CustomerDetail>(It.IsAny<Customer>())).Returns(customerOne);
            var result = await _customerRepo.GetCustomerByIdAsync(customerIdToGet);

            dbContextTest.Database.EnsureDeleted();
            // Assert.
            Assert.NotNull(result);
            result.CustomerId.Equals(1);
        }

        [Fact]
        public async void GetCustomerById_returns_notfound_when_data_doesnot_exists()
        {
            // Arrange.
            long customerIdToGet = 999;
            this._mapper.Setup(x => x.Map<CustomerDetail>(It.IsAny<Customer>())).Returns(() => null);
            var result = await _customerRepo.GetCustomerByIdAsync(customerIdToGet);

            dbContextTest.Database.EnsureDeleted();
            // Assert.
            Assert.Null(result);
        }

        [Fact]
        public async void GetCustomers_returns_customers_when_data_exists()
        {
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

            // Arrange.            
            this._mapper.Setup(x => x.Map<List<CustomerDetail>>(It.IsAny<List<Customer>>())).Returns(customerList);
            var result = await _customerRepo.GetRegisteredCustomersAsync();

            dbContextTest.Database.EnsureDeleted();
            // Assert.
            Assert.NotNull(result);
            result.Customers.Should().HaveCount(2);
        }

        [Fact]
        public async void GetCustomers_returns_no_customers_when_data_doesnot_exists()
        {
            // Arrange.
            var customerList = new List<CustomerDetail>();
            this._mapper.Setup(x => x.Map<List<CustomerDetail>>(It.IsAny<List<Customer>>())).Returns(customerList);
            var result = await _customerRepo.GetRegisteredCustomersAsync();

            dbContextTest.Database.EnsureDeleted();
            // Assert.
            result.Customers.Should().HaveCount(0);
        }

        [Fact]
        public async void RegisterCustomer_returns_true_when_new_customer()
        {
            // Arrange.
            var customerThree = new CustomerRequest
            {
                FullName = "jhanu shannu",
                Address = "classic",
            };
            var result = await _customerRepo.RegisterCustomerAsync(customerThree);

            dbContextTest.Database.EnsureDeleted();
            // Assert.
            result.Equals(true);
        }

        [Fact]
        public async void RegisterCustomer_returns_false_when_existing_customer()
        {
            // Arrange.
            var customerThree = new CustomerRequest
            {
                FullName = "uma",
                Address = "classic",
            };
            var result = await _customerRepo.RegisterCustomerAsync(customerThree);

            dbContextTest.Database.EnsureDeleted();
            // Assert.
            result.Equals(false);
        }

        [Fact]
        public async void UpdateCustomer_returns_true_when_existing_customer()
        {
            // Arrange.
            var customerOne = new UpdateCustomerRequest
            {
                FullName = "uma1",
                Address = "classic",
            };
            var result = await _customerRepo.UpdateCustomerAsync(1, customerOne);

            dbContextTest.Database.EnsureDeleted();
            // Assert.
            result.Equals(true);
        }

        [Fact]
        public async void UpdateCustomer_returns_false_when_no_customer()
        {
            // Arrange.
            var customerFour = new UpdateCustomerRequest
            {
                FullName = "new customer",
                Address = "classic",
            };
            var result = await _customerRepo.UpdateCustomerAsync(4, customerFour);

            dbContextTest.Database.EnsureDeleted();
            // Assert.
            result.Equals(false);
        }

        [Fact]
        public async void DeleteCustomer_returns_true_when_existing_customer()
        {
            // Arrange.
            var result = await _customerRepo.DeleteCustomerAsync(9);

            dbContextTest.Database.EnsureDeleted();
            // Assert.
            result.Equals(true);
        }
    }
}