using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CustomerDomainModel.Models;
using CustomerRepository.Interfaces;
using CustomerAPI.Models;
namespace CustomerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        /// <summary>
        /// Customer Repository.
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Get customers.
        /// </summary>
        /// <returns>customers</returns>
        [HttpGet("RegisteredCustomers")]
        public async Task<ActionResult> GetRegisteredCustomersAsync()
        {
            var registeredCustomers = await _customerRepository.GetRegisteredCustomersAsync().ConfigureAwait(false);
            return registeredCustomers.Customers != null
            ? Ok(registeredCustomers.Customers)
            : StatusCode(Constants.UnprocessableEntityCode, Errors.NoRecordsFound);
        }

        /// <summary>
        /// Get customer by id.
        /// </summary>
        /// <param name="customerId">customerId.</param>
        /// <returns>true or false</returns>
        [HttpGet("{customerId}")]
        public async Task<ActionResult> GetCustomerByIdAsync(long customerId)
        {
            if (customerId <= 0)
            {
                return BadRequest(Errors.BadRequest);
            }
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId).ConfigureAwait(false);
            return customer != null
            ? Ok(customer)
            : StatusCode(Constants.UnprocessableEntityCode, Errors.NoCustomer);
        }

        /// <summary>
        /// Register Customer.
        /// </summary>
        /// <param name="request">Customer request.</param>
        /// <returns>true or false</returns>
        [HttpPost("registration")]
        public async Task<IActionResult> RegisterCustomerAsync([FromBody] CustomerRequest request)
        {
            if (string.IsNullOrEmpty(request.FullName))
            {
                return BadRequest(Errors.BadRequest);
            }
            return await _customerRepository.RegisterCustomerAsync(request).ConfigureAwait(false)
                ? StatusCode(Constants.SuccessCode, Constants.CustomerRegistrationMessage)
                : StatusCode(Constants.UnprocessableEntityCode, Errors.CustomerExists);
        }

        /// <summary>
        /// Update Customer.
        /// </summary>
        /// <param name="customerId">customerId.</param>
        /// <param name="request">Customer request.</param>
        /// <returns>true or false</returns>
        [HttpPut("update/{customerId}")]
        public async Task<IActionResult> UpdateCustomerAsync(long customerId, [FromBody] UpdateCustomerRequest request)
        {
            if (customerId <= 0)
            {
                return BadRequest(Errors.BadRequest);
            }
            return await _customerRepository.UpdateCustomerAsync(customerId, request).ConfigureAwait(false)
                ? StatusCode(Constants.SuccessCode, Constants.CustomerUpdateMessage)
                : StatusCode(Constants.UnprocessableEntityCode, Errors.NoCustomer);
        }

        /// <summary>
        /// Delete Customer.
        /// </summary>
        /// <param name="customerId">customerId.</param>
        /// <returns>true or false</returns>
        [HttpDelete("delete/{customerId}")]
        public async Task<IActionResult> DeleteCustomerAsync(long customerId)
        {
            if (customerId <= 0)
            {
                return BadRequest(Errors.BadRequest);
            }
            return await _customerRepository.DeleteCustomerAsync(customerId).ConfigureAwait(false)
                ? StatusCode(Constants.SuccessCode, Constants.CustomerDeletionMessage)
                : StatusCode(Constants.UnprocessableEntityCode, Errors.NoCustomer);
        }
    }
}
