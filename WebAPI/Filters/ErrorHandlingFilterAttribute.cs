using CleanArchitecture.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CleanArchitecture.WebAPI.Filters
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        private ILogger<ErrorHandlingFilterAttribute> _logger;

        public ErrorHandlingFilterAttribute(ILogger<ErrorHandlingFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            _logger.LogError(exception, exception.Message);

            var problemDetails = new ProblemDetails();
            switch (exception)
            {
                case NonExistentDeviceException:
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Detail = exception.Message;
                    break;
                case AppException:
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Detail = exception.Message;
                    break;
                default:
                    problemDetails.Status = (int)HttpStatusCode.InternalServerError;
                    problemDetails.Detail = "Something went wrong, we are working on it.";
                    break;
            }

            context.Result = new ObjectResult(problemDetails);
        }
    }
}
