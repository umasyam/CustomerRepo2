namespace CustomerDomainModel.Models
{
    /// <summary>
    /// Customer class.
    /// </summary>
    public class UpdateCustomerRequest
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

        /// <summary>
        /// Date Of Registration.
        /// </summary>
        public System.DateTime DateOfRegistration { get; set; }

        /// <summary>
        /// Is Customer Active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}