using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventManager.Infrastructure.Middlewares
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, 
            ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex) 
            { 
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleException(HttpContext httpContext, Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception. Method={Method}, Path={Path}, RequestId={RequestId}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.Request.Headers["x-request-id"]);

            //Обеспечьте корректные HTTP-статусы для разных типов ошибок:
            //400 Bad Request для ошибок валидации;
            //404 Not Found для ситуаций, когда ресурс не найден;
            //500 Internal Server Error для непредвиденных ошибок.

            if (httpContext.Response.HasStarted)
            {
                return;
            }

            var statusCode = MapStatusCode(ex);
            
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            var error = new ProblemDetails
            {
                Status = statusCode,
                Detail = ex.Message
            };
            await httpContext.Response.WriteAsJsonAsync(error);
        }

        private static int MapStatusCode(Exception ex) => ex switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        
    }
}
