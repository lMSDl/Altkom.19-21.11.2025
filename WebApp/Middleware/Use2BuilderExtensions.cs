namespace WebApp.Middleware
{
    public static class Use2BuilderExtensions
    {
        extension(IApplicationBuilder builder)
        {
            public IApplicationBuilder Use2()
            {
                builder.UseMiddleware<Use2Middleware>();
                return builder;
            }
        }
    }
}
