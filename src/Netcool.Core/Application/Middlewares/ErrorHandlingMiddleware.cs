using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Netcool.Core.Entities;

namespace Netcool.Core.Application.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            var statusCode = HttpStatusCode.InternalServerError; // 500 if unexpected
            var errorCode = 0;

            if (exception is UserFriendlyException e)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorCode = e.Code;
            }
            else if (exception is EntityNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else if (exception is ArgumentException || exception is ApplicationException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }

            logger.LogError(exception, exception.Message);

            var result = JsonSerializer.Serialize(new {Code = errorCode, exception.Message});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;
            return context.Response.WriteAsync(result, Encoding.UTF8);
        }
    }
}