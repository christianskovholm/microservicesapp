using System;
using System.Collections.Generic;
using System.Net;
using Domain.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using OrganizationService.Application;
using OrganizationService.Application.Responses;
using Xunit;

namespace OrganizationService.UnitTests
{
    public class HttpGlobalExceptionFilterTests
    {
        private readonly HttpGlobalExceptionFilter _filter;
        private ActionContext _actionContext { get; set; }


        public HttpGlobalExceptionFilterTests()
        {
            _filter = new HttpGlobalExceptionFilter(new Mock<ILogger<HttpGlobalExceptionFilter>>().Object);
            _actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>()
            );
        }

        [Fact]
        public void OnException_Should_Set_ExceptionContext_To_Not_Found()
        {
            // Arrange
            var exception = new DomainObjectNotFoundException("test");
            var exceptionContext = new ExceptionContext(_actionContext, new List<IFilterMetadata>()) { Exception = exception };

            // Act
            _filter.OnException(exceptionContext);

            // Assert
            var result = exceptionContext.Result as ObjectResult;

            exceptionContext.ExceptionHandled.Should().Be(true);
            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            (result.Value as HttpErrorResponse).Error.Code.Should().Be((int)HttpStatusCode.NotFound);
            (result.Value as HttpErrorResponse).Error.Message.Should().Be(exception.Message);
        }

        [Fact]
        public void OnException_Should_Set_ExceptionContext_To_Internal_Server_Error()
        {
            // Arrange
            var exception = new Exception("test");
            var exceptionContext = new ExceptionContext(_actionContext, new List<IFilterMetadata>()) { Exception = exception };

            // Act
            _filter.OnException(exceptionContext);

            // Assert
            var result = exceptionContext.Result as ObjectResult;

            exceptionContext.ExceptionHandled.Should().Be(true);
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            (result.Value as HttpErrorResponse).Error.Code.Should().Be((int)HttpStatusCode.InternalServerError);
            (result.Value as HttpErrorResponse).Error.Message.Should().Be(HttpGlobalExceptionFilter.InternalServerErrorMessage);
        }
    }
}