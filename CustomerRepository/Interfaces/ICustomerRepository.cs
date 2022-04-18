using System.Threading.Tasks;
using CustomerDomainModel.Models;
using System.Collections.Generic;
namespace CustomerRepository.Interfaces
{
    /// <summary>
    /// Customer Repository interface.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Gets All Customers.
        /// </summary>
        /// <returns>All Customers.</returns>
        Task<GetCustomersResponse> GetRegisteredCustomersAsync();

        /// <summary>
        /// Registers Customer.
        /// </summary>
        /// <param name="customer">customer.</param>
        /// <returns>true or false.</returns>
        Task<bool> RegisterCustomerAsync(CustomerRequest customer);

        /// <summary>
        /// Update Customer.
        /// </summary>
        /// <param name="customerId">customerId.</param>
        /// <param name="customer">customer.</param>
        /// <returns>true or false.</returns>
        Task<bool> UpdateCustomerAsync(long customerId, UpdateCustomerRequest customer);

        /// <summary>
        /// Delete Customer.
        /// </summary>
        /// <param name="customerId">customerId.</param>
        /// <returns>true or false.</returns>
        Task<bool> DeleteCustomerAsync(long customerId);

        /// <summary>
        /// Get registered customer by id.
        /// </summary>
        /// <param name="customerId">customerId.</param>
        /// <returns>Registered customer.</returns>
        Task<CustomerDetail> GetCustomerByIdAsync(long customerId);
    }
}