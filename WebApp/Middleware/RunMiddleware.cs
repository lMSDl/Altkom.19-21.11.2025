
namespace WebApp.Middleware
{
    public class RunMiddleware : IMiddleware
    {
        //nie uzywamy RequestDelegate ponieważ jest to terminal middleware
        //parametr _ jest tylko po to aby spełnić sygnaturę metody InvokeAsync z interfejsu IMiddleware
        public async Task InvokeAsync(HttpContext context, RequestDelegate _)
        {
            Console.WriteLine("Run: Hello World!");
            await context.Response.WriteAsync("Hello World!");
        }
    }
}
