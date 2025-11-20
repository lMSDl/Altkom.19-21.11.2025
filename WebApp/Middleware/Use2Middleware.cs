using System;

namespace WebApp.Middleware
{
    public class Use2Middleware
    {
        private readonly RequestDelegate _next;
        public Use2Middleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            Console.WriteLine("Begin of Use2");
            await _next(httpContext);
            Console.WriteLine("End of Use2");
        }

    }
}
