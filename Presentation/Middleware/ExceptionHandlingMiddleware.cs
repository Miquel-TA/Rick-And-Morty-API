using Domain.Models;
using System.Text.Json;

namespace Presentation.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType);

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _log.Error($"Identifier {context.TraceIdentifier} got error {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError; // Will return StatusCode 500 for any type of exception to simplify http codes

            var errorResponse = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode, // Inside the ErrorResponse will contain the real StatusCode.
                Message = exception.Message,
                Timestamp = DateTime.UtcNow
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
