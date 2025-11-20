var builder = WebApplication.CreateBuilder(args);
//miejsce na konfiguracjê DI, Loggera, Konfiguracji

//wbudowana konfiguracja
//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//³adowanie opcjonalnego pliku konfiguracyjnego zale¿nego od œrodowiska
//builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

//dostep do konfiguracji
//builder.Configuration

//dostep do kofiguracji DI - ServiceCollection
//builder.Services.

//dostep do konfiguracji loggera
//builder.Logging


var app = builder.Build();
//miejsce na konfiguracjê œcie¿ki przetwarzania ¿¹dañ

//domyœlnie, jeœli jest œrodowisko deweloperskie, to w³¹cza stronê b³êdów deweloperskich na której s¹ szczegó³y wyj¹tku wraz z stack trace
//w przeciwnym wypadku jest strona b³êdu 500 (Internal Serrver Error)
/*if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}*/

//dostep do konfiguracji
//app.Configuration

//dostep do DI - IServiceProvider
//app.Services


//sposoby sprawdzania œrodowiska
if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => "Hello from development!");
}
else if(app.Environment.IsStaging())
{
    app.MapGet("/", () => "Hello from staging!");
}
else if(app.Environment.IsProduction())
{
    app.MapGet("/", () => "Hello from production!");
}
else if (app.Environment.IsEnvironment("Alamakota"))
{
    app.MapGet("/", () => "Hello from alamakota!");
}

//test developer exception page
app.MapGet("/error", x => throw new Exception());

//wstrzykiwanie loggera do metody anonimowej
//app.MapGet("/", (ILogger<Program> logger) => { logger.LogDebug("ala ma kota"); return "Hello World!"; });

app.Run();
