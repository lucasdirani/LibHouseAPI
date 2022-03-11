using KissLog;
using LibHouse.API.Extensions.Common;
using LibHouse.API.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LibHouse.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _nextRequest;

        public ExceptionMiddleware(RequestDelegate nextRequest)
        {
            _nextRequest = nextRequest;
        }

        public async Task InvokeAsync(HttpContext httpContext, IKLogger logger)
        {
            try
            {
                await _nextRequest(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex, httpContext);

                logger.Log(LogLevel.Error, ex);
            }
        }

        private async static Task HandleExceptionAsync(
            Exception exception,
            HttpContext httpContext)
        {
            if (!httpContext.Response.HasStarted)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                httpContext.Response.ContentType = "application/problem+json";

                ProblemDetails exceptionResponse = httpContext.BuildResponseFromException(exception);

                string jsonException = exceptionResponse.ToJson();

                await httpContext.Response.WriteAsync(jsonException);
            }
        }
    }
}