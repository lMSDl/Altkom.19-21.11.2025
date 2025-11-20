
namespace WebApp.Middleware
{
    public class Use1Middleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("Begin of Use1");

            await next(context);

            Console.WriteLine("End of Use1");
        }
    }
}
