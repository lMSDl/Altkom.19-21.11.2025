namespace WebApp.Middleware
{
    public class HelloRunMiddleware
    {
        //nie uzywamy RequestDelegate ponieważ jest to terminal middleware
        //parametr _ jest tylko po to aby spełnić sygnaturę konstruktora przy użyciu UseMiddleware<T>
        public HelloRunMiddleware(RequestDelegate _)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("Run: Hello from HelloRunMiddleware!");
            await context.Response.WriteAsync("Hello from HelloRunMiddleware!");
        }
    }
}
