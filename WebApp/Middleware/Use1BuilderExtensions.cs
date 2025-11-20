namespace WebApp.Middleware
{
    public static class Use1BuilderExtensions
    {
        public static IApplicationBuilder Use1(this IApplicationBuilder app)
        {
            app.UseMiddleware<Use1Middleware>();
            return app;
        }
    }
}
