using System.Collections.Generic;
namespace CustomerDomainModel.Models
{
    /// <summary>
    /// GetCustomersResponse.
    /// </summary>
    public class GetCustomersResponse
    {
        /// <summary>
        /// Gets all Customers.
        /// </summary>
        public List<CustomerDetail> Customers { get; set; }
    }
}