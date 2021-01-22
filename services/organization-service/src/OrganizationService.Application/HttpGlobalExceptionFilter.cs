using System.Net;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OrganizationService.Application.Responses;

namespace OrganizationService.Application
{
    /// <summary>
    /// IExceptionFilter implementation that is triggered upon runtime exceptions during requests.
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public static string InternalServerErrorMessage = "An error occurred. Please try again.";
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Adds an ObjectResult, containing information about the exception, to the Result of the
        /// exception context.
        /// </summary>
        public void OnException(ExceptionContext context)
        {
            var eventId = new EventId(context.Exception.HResult);
            var exceptionType = context.Exception.GetType();
            var json = new JObject();
            LogLevel logLevel;
            string message;
            int statusCode;

            if (exceptionType == typeof(DomainException) || exceptionType == typeof(DomainObjectNotFoundException))
            {
                statusCode =  exceptionType == typeof(DomainException) ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.NotFound;
                message = context.Exception.Message;
                logLevel = LogLevel.Information;
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = InternalServerErrorMessage;
                logLevel = LogLevel.Error;
            }

            _logger.Log(logLevel, eventId, context.Exception.Message);

            var response = new HttpErrorResponse(statusCode, message);
            context.Result = new ObjectResult(response) { StatusCode = statusCode };
            context.ExceptionHandled = true;
        }
    }
}