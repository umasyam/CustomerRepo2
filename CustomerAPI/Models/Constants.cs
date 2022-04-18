namespace CustomerAPI.Models
{
    /// <summary>
    /// Constants class.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Bad Request Message.
        /// </summary>
        internal const string BadRequestMessage = "Mandatory request parameters are not supplied";

        /// <summary>
        /// Customer exists Message.
        /// </summary>
        internal const string CustomerExistsMessage = "This Customer already exists.";

        /// <summary>
        /// Customer registration Message.
        /// </summary>
        internal const string CustomerRegistrationMessage = "Customer Registration is successful.";

        /// <summary>
        /// Customer Updation Message.
        /// </summary>
        internal const string CustomerUpdateMessage = "Customer detail Updation is successful.";

        /// <summary>
        /// Customer Deletion Message.
        /// </summary>
        internal const string CustomerDeletionMessage = "Customer Deletion is successful.";


        /// <summary>
        /// No Records Found Message.
        /// </summary>
        internal const string NoRecordsFound = "No Records Found.";

        /// <summary>
        /// No NoCustomer Found Message.
        /// </summary>
        internal const string NoCustomer = "No customer found with the provided customer id.";

        /// <summary>
        /// Success Code.
        /// </summary>
        internal const int SuccessCode = 201;

        /// <summary>
        /// Unprocessable Entity Code.
        /// </summary>
        internal const int UnprocessableEntityCode = 422;

        /// <summary>
        /// Technical Failure Message.
        /// </summary>
        internal const string TechnicalFailureMessage = "Encountered technical failure.";

         /// <summary>
        /// Error Message.
        /// </summary>
        internal const string TechnicalErrorMessage = "Encountered technical error in the underlying service call.";

    }
}