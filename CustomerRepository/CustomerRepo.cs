using System;
using System.Threading.Tasks;
using CustomerRepository.Interfaces;
using CustomerRepository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CustomerDomainModel.Models;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
namespace CustomerRepository
{
    /// <summary>
    /// Customer Repository class.
    /// </summary>
    public class CustomerRepo : ICustomerRepository
    {
        /// <summary>
        /// The auto mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// User dabase context.
        /// </summary>
        private readonly UserDbContext _userDbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="userDbContext">User database context.</param>
        /// <param name="mapper">Auto Mapper.</param>
        public CustomerRepo(UserDbContext userDbContext, IMapper mapper)
        {
            _userDbContext = userDbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Registered Customers.
        /// </summary>
        /// <returns>Registered customers.</returns>
        public async Task<GetCustomersResponse> GetRegisteredCustomersAsync()
        {
            var customers = await _userDbContext.Customers.ToListAsync().ConfigureAwait(false);
            return new GetCustomersResponse { Customers = _mapper.Map<List<CustomerDetail>>(customers) };
            //return _mapper.Map<List<CustomerDetail>>(customers);
        }

        /// <summary>
        /// Get registered customer by id.
        /// </summary>
        /// <param name="customerId">customerId.</param>
        /// <returns>Registered customer.</returns>
        public async Task<CustomerDetail> GetCustomerByIdAsync(long customerId)
        {
            var exists = await _userDbContext.Customers.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId)).ConfigureAwait(false);
            if (exists == null)
            {
                return null;
            }
            return _mapper.Map<CustomerDetail>(exists);
        }

        /// <summary>
        /// Registers Customer.
        /// </summary>
        /// <param name="customer">customer.</param>
        /// <returns>true or false.</returns>
        public async Task<bool> RegisterCustomerAsync(CustomerRequest customer)
        {
            var customers = await _userDbContext.Customers.ToListAsync().ConfigureAwait(false);
            var alreadyexist = customers.FirstOrDefault(p => p.FullName.ToUpperInvariant().Equals(customer.FullName.ToUpperInvariant()));
            //Customer already exists
            //var alreadyexist = await _userDbContext.Customers.FirstOrDefaultAsync(p => p.FullName.ToUpperInvariant().Equals(customer.FullName.ToUpperInvariant())).ConfigureAwait(false);
            if (alreadyexist != null)
            {
                return false;
            }
            var item = new Customer
            {
                FullName = customer.FullName,
                Address = customer.Address,
                DateOfBirth = customer.DateOfBirth,
                DateOfRegistration = DateTime.Now,
                IsActive = true
            };
            _userDbContext.Customers.Add(item);
            return await _userDbContext.SaveChangesAsync().ConfigureAwait(false) == 1;
        }

        /// <summary>
        /// Update Customer.
        /// </summary>
        /// <param name="customerId">customerId.</param>
        /// <param name="customer">customer.</param>
        /// <returns>true or false.</returns>
        public async Task<bool> UpdateCustomerAsync(long customerId, UpdateCustomerRequest customer)
        {
            var exists = await _userDbContext.Customers.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId)).ConfigureAwait(false);
            if (exists == null)
            {
                return false;
            }
            exists.FullName = customer.FullName;
            exists.Address = customer.Address;
            exists.DateOfBirth = customer.DateOfBirth;
            exists.DateOfRegistration = customer.DateOfRegistration;
            exists.IsActive = customer.IsActive;
            //_userDbContext.Customers.Add(item);
            //_userDbContext.Entry(item).State = EntityState.Modified;
            return await _userDbContext.SaveChangesAsync().ConfigureAwait(false) == 1;
        }

        /// <summary>
        /// Delete Customer.
        /// </summary>
        /// <param name="customerId">customerId.</param>
        /// <returns>true or false.</returns>
        public async Task<bool> DeleteCustomerAsync(long customerId)
        {
            var exists = await _userDbContext.Customers.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId)).ConfigureAwait(false);
            if (exists == null)
            {
                return false;
            }
            //Soft delete by making IsActive as false :)
            exists.IsActive = false;
            return await _userDbContext.SaveChangesAsync().ConfigureAwait(false) == 1;
        }
    }
}