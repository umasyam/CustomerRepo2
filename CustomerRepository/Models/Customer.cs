using System.ComponentModel.DataAnnotations.Schema;
namespace CustomerRepository.Models
{
    /// <summary>
    /// Customer class.
    /// </summary>
    [Table("Customers")]
    public class Customer
    {
        /// <summary>
        /// Customer Id.
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [Column("CustomerId")]
        public long CustomerId { get; set; }

        /// <summary>
        /// Full Name.
        /// </summary>
        [Column("FullName")]
        public string FullName { get; set; }

        /// <summary>
        /// Date Of Birth.
        /// </summary>
        [Column("DateOfBirth")]
        public System.DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        [Column("Address")]
        public string Address { get; set; }

        /// <summary>
        /// Date Of Registration.
        /// </summary>
        [Column("DateOfRegistration")]
        public System.DateTime DateOfRegistration { get; set; }

        /// <summary>
        /// Is Customer Active.
        /// </summary>
        [Column("IsActive")]
        public bool IsActive { get; set; }

        /// <summary>
        /// The created date.
        /// </summary>
        [Column("created_date")]
        public System.DateTime CreatedDate { get; set; }

        /// <summary>
        /// The updated date.
        /// </summary>
        [Column("updated_date")]
        public System.DateTime UpdatedDate { get; set; }
    }
}