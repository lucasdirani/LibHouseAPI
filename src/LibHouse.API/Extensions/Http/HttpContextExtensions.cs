using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace LibHouse.API.Extensions.Http
{
    public static class HttpContextExtensions
    {
        public static ProblemDetails BuildResponseFromException(
            this HttpContext httpContext,
            Exception exception)
            => new()
            {
                Detail = exception.StackTrace,
                Title = exception.Message,
                Status = (int) HttpStatusCode.InternalServerError,
                Instance = httpContext.Request.Path.Value
            };
    }
}