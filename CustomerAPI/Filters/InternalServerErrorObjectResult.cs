using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace CustomerAPI.Filters
{
    /// <summary>
    /// Internal server Error Object Result.
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerErrorObjectResult" /> class.
        /// </summary>
        /// <param name="error">Error value.</param>
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}