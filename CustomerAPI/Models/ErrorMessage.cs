namespace CustomerAPI.Models
{
    /// <summary>
    /// Error Message Class.
    /// </summary>
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }
    }
}
