using Bogus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services.Bogus;
using Services.Bogus.Fakers;
using Services.InMemory;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
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

/*builder.Services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/cookie";
        *//*options.LogoutPath = "/logout";*//*
        options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
    });*/


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
app.MapGet("/users/{id:int}", [Authorize] async (ICrudService<User> usersService, int id, HttpContext context) =>
{
    
    var user = await usersService.ReadAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});
app.MapPost("/users", [Authorize(Roles = nameof(Roles.Create))] async (ICrudService<User> usersService, User user) =>
{
    var userId = await usersService.CreateAsync(user);
    return Results.Created($"/users/{userId}", user);
});

//app.MapPut("/users/{id:int}", [Authorize(Roles = "Edit")] async (ICrudService<User> usersService, int id, User user) => //pojedyncza rola w pojedynczym Authorize - wymagana rola
//app.MapPut("/users/{id:int}", [Authorize(Roles = "Create, Delete")] async (ICrudService<User> usersService, int id, User user) => //wiele ról po przecinku w pojedynczym Authorize - wymagana jedna z wymienionych ról
app.MapPut("/users/{id:int}", [Authorize(Roles = "Create")][Authorize(Roles = "Delete")] async (ICrudService<User> usersService, int id, User user) => //wiele ról w wielu Authorize - wymagana kazda z ról
{
    var existingUser = await usersService.ReadAsync(id);
    if (existingUser is null)
    {
        return Results.NotFound();
    }
    await usersService.UpdateAsync(id, user);
    return Results.NoContent();
});
app.MapDelete("/users/{id:int}", [Authorize(Roles = nameof(Roles.Delete))] async (ICrudService<User> usersService, int id) =>
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

    ICollection<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

    foreach (string role in user.Roles.ToString().Split(", "))
    {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }


    var tockenDescriptor = new SecurityTokenDescriptor
    {
        Expires = DateTime.UtcNow.AddMinutes(30),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Auth:Key"])), SecurityAlgorithms.HmacSha256),
        Issuer = config["Auth:Issuer"],
        Audience = config["Auth:Audience"],
        Subject = new ClaimsIdentity(claims)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tockenDescriptor);

    return Results.Ok(tokenHandler.WriteToken(token));
});

//username i password w query string dla uproszczenia wywo³ania w przegl¹darce
app.MapGet("/cookie", async (HttpContext context, IAuth authService, string username, string password, string returnUrl) =>
{
    var user = await authService.GetAsync(username, password);
    if (user is null)
    {
        return Results.Unauthorized();
    }

    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync(principal);
    return Results.Redirect(returnUrl);
});

app.Run();
