var builder = WebApplication.CreateBuilder(args);


//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//dostep do konfiguracji
//builder.Configuration

//dostep do kofiguracji DI - ServiceCollection
//builder.Services.

//dostep do konfiguracji loggera
//builder.Logging


var app = builder.Build();

//dostep do konfiguracji
//app.Configuration

//dostep do DI - IServiceProvider
//app.Services

app.MapGet("/", () => "Hello World!");

app.Run();
