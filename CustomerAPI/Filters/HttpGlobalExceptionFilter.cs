using System.Net;
using System.Net.Http;
using CustomerAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using FluentValidation;
namespace CustomerAPI.Filters
{
    /// <summary>
    /// Http Global Exception Filter.
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// Web Host Environment.
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Http Global Exception Filter Logger.
        /// </summary>
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpGlobalExceptionFilter" /> class.
        /// </summary>
        /// <param name="env">Web Host Environment.</param>
        /// <param name="logger">Http Global Exception Filter Logger.</param>
        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this._env = env;
            this._logger = logger;
        }

        /// <summary>
        /// On Exception method.
        /// </summary>
        /// <param name="context">Exception Context.</param>
        public void OnException(ExceptionContext context)
        {
            // Log the Exception.
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            var exception = context.Exception;

            switch (exception)
            {
                // Set StatusCode to 422 When Records Not Found in DB.
                case RecordNotFoundException _:
                    context.Result = new BadRequestObjectResult(Errors.NoRecordsFound)
                    {
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    };
                    break;
                case ValidationException _:
                    context.Result = new BadRequestObjectResult(Errors.BadRequest);
                    break;
                case HttpRequestException _:
                    context.Result = new BadRequestObjectResult(Errors.TechnicalServiceError)
                    {
                        StatusCode = StatusCodes.Status422UnprocessableEntity
                    };
                    break;
                default:
                {
                    const int internalServerError = (int)HttpStatusCode.InternalServerError;

                    context.Result = new InternalServerErrorObjectResult(Errors.TechnicalFailure);

                    context.HttpContext.Response.StatusCode = internalServerError;
                    break;
                }
            }
            context.ExceptionHandled = true;
        }
    }
}
