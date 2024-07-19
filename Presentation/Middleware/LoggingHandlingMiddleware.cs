using System.Diagnostics;

namespace Presentation.Middleware
{
    public class LoggingHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType);

        public LoggingHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Log the incoming request
            _log.Info($"Identifier {context.TraceIdentifier} requested {context.Request.Method} {context.Request.Path}");

            await _next(context);

            // Log the outgoing response
            stopwatch.Stop();
            _log.Info($"Identifier {context.TraceIdentifier} got response {context.Response.StatusCode} in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
