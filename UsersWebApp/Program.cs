using Bogus;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.InMemory;
using Services.Interfaces;
using UsersWebApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<ICrudService<User>, BogusCrudService<User>>();
    builder.Services.AddTransient<Faker<User>, UserFaker>();
    builder.Services.AddTransient<BogusConfig>(x => x.GetRequiredService<IConfiguration>().GetSection(nameof(Bogus)).Get<BogusConfig>()!);
}
else
{
    builder.Services.AddSingleton<ICrudService<User>, InMemoryCrudService<User>>();
}

builder.Services.AddTransient<LoggerMiddleware>();

var app = builder.Build();

app.UseLogger();

//ten sam wynik co za pomoc¹ MapGet, ale brak mo¿liwoœci wstrzykiwania zale¿noœci, adnotacji itp.
/*app.Map("/users", usersApp =>
{
    usersApp.Run(async context =>
    {
        var usersService = context.RequestServices.GetRequiredService<ICrudService<User>>();
        var users = await usersService.ReadAsync();
            
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsJsonAsync(users);
    });
});*/

app.MapGet("/users", (ICrudService<User> usersService) => usersService.ReadAsync());
app.MapGet("/users/{id:int}", async (ICrudService<User> usersService, int id) =>
{
    var user = await usersService.ReadAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});
app.MapPost("/users", async (ICrudService<User> usersService, User user) =>
{
    var userId = await usersService.CreateAsync(user);
    return Results.Created($"/users/{userId}", user);
});
app.MapPut("/users/{id:int}", async (ICrudService<User> usersService, int id, User user) =>
{
    var existingUser = await usersService.ReadAsync(id);
    if (existingUser is null)
    {
        return Results.NotFound();
    }
    await usersService.UpdateAsync(id, user);
    return Results.NoContent();
});
app.MapDelete("/users/{id:int}", async (ICrudService<User> usersService, int id) =>
{
    var existingUser = await usersService.ReadAsync(id);
    if (existingUser is null)
    {
        return Results.NotFound();
    }
    await usersService.DeleteAsync(id);
    return Results.NoContent();
});


app.Run();
