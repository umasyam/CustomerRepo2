namespace CustomerAPI.Models
{
    /// <summary>
    /// Errors class.
    /// </summary>
    public static class Errors
    {
        /// <summary>
        /// Error for Bad Request.
        /// </summary>
        public static ErrorMessage BadRequest => new ErrorMessage(Constants.BadRequestMessage);

        /// <summary>
        /// Error for duplicate ustomer.
        /// </summary>
        public static ErrorMessage CustomerExists => new ErrorMessage(Constants.CustomerExistsMessage);

        /// <summary>
        /// Error for No records found.
        /// </summary>
        public static ErrorMessage NoRecordsFound => new ErrorMessage(Constants.NoRecordsFound);

        /// <summary>
        /// Error for No records found.
        /// </summary>
        public static ErrorMessage NoCustomer => new ErrorMessage(Constants.NoCustomer);

        /// <summary>
        /// Error for Technical Failure.
        /// </summary>
        public static ErrorMessage TechnicalFailure => new ErrorMessage(Constants.TechnicalFailureMessage);

        /// <summary>
        /// Error for Technical Service Failure - service returns a non-200 status code.
        /// </summary>
        public static ErrorMessage TechnicalServiceError => new ErrorMessage(Constants.TechnicalErrorMessage);
    }
}