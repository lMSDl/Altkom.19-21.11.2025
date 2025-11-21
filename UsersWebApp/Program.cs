using Bogus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.InMemory;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UsersWebApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<BogusUserService>();
    builder.Services.AddTransient<ICrudService<User>>(x => x.GetRequiredService<BogusUserService>());
    builder.Services.AddTransient<IAuth>(x => x.GetRequiredService<BogusUserService>());

    builder.Services.AddTransient<Faker<User>, UserFaker>();
    builder.Services.AddTransient<BogusConfig>(x => x.GetRequiredService<IConfiguration>().GetSection(nameof(Bogus)).Get<BogusConfig>()!);
}
else
{
    builder.Services.AddSingleton<ICrudService<User>, InMemoryCrudService<User>>();
}

builder.Services.AddTransient<LoggerMiddleware>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Auth:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Auth:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:Key"])),
        ValidateLifetime = true,

        //domyœlnie tokeny maj¹ 5 minut tolerancji czasu ¿ycia, co pozwala na synchronizacjê czasu miêdzy serwerem a klientem
        //tutaj ustawiamy na zero, co oznacza brak tolerancji - token wygaœnie dok³adnie w momencie okreœlonym w polu Expire
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddAuthorization();

var app = builder.Build();

app.UseLogger();

app.UseAuthentication();
app.UseAuthorization();

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

app.MapGet("/users", [Authorize] (ICrudService<User> usersService) => usersService.ReadAsync());
app.MapGet("/users/{id:int}", [Authorize] async (ICrudService<User> usersService, int id) =>
{
    var user = await usersService.ReadAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});
app.MapPost("/users", [Authorize] async (ICrudService<User> usersService, User user) =>
{
    var userId = await usersService.CreateAsync(user);
    return Results.Created($"/users/{userId}", user);
});
app.MapPut("/users/{id:int}", [Authorize] async (ICrudService<User> usersService, int id, User user) =>
{
    var existingUser = await usersService.ReadAsync(id);
    if (existingUser is null)
    {
        return Results.NotFound();
    }
    await usersService.UpdateAsync(id, user);
    return Results.NoContent();
});
app.MapDelete("/users/{id:int}", [Authorize] async (ICrudService<User> usersService, int id) =>
{
    var existingUser = await usersService.ReadAsync(id);
    if (existingUser is null)
    {
        return Results.NotFound();
    }
    await usersService.DeleteAsync(id);
    return Results.NoContent();
});


app.MapPost("/login", async (IConfiguration config, IAuth authService, User credential) =>
{
    var user = await authService.GetAsync(credential.Username, credential.Password);
    if (user is null)
        return Results.Unauthorized();

    var tockenDescriptor = new SecurityTokenDescriptor
    {
        Expires = DateTime.UtcNow.AddSeconds(30),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Auth:Key"])), SecurityAlgorithms.HmacSha256),
        Issuer = config["Auth:Issuer"],
        Audience = config["Auth:Audience"]
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tockenDescriptor);

    return Results.Ok(tokenHandler.WriteToken(token));
});

app.Run();
