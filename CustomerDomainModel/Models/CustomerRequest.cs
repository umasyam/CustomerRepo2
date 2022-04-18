using System.ComponentModel.DataAnnotations.Schema;
namespace CustomerDomainModel.Models
{
    /// <summary>
    /// CustomerRequest class.
    /// </summary>
    public class CustomerRequest
    {
        /// <summary>
        /// Full Name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Date Of Birth.
        /// </summary>
        public System.DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        public string Address { get; set; }
    }
}