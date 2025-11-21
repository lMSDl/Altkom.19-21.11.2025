
using System.Diagnostics;

namespace UsersWebApp.Middleware
{
    public class LoggerMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public LoggerMiddleware(ILogger<LoggerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _logger.LogInformation("Request: {Method} {Path}", context.Request.Method, context.Request.Path);

            var timer = Stopwatch.StartNew();
            await next(context);
            timer.Stop();

            _logger.LogInformation("Response: {Code} ({Time} ms)", context.Response.StatusCode, timer.ElapsedMilliseconds);


        }
    }

    public static class LoggerMiddlewareAppBuilderExtensions
    {
        extension(IApplicationBuilder builder)
        {
            public IApplicationBuilder UseLogger()
            {
                return builder.UseMiddleware<LoggerMiddleware>();
            }
        }
    }
}
