using WebApp.Middleware;

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

builder.Services.AddTransient<Use1Middleware>();
builder.Services.AddTransient<RunMiddleware>();


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



//middleware - mo¿liwoœæ wykonania akcji przed i po przekazaniu kontekstu do kolejnego middeware
/*app.Use(async (context, next) =>
{

    Console.WriteLine("Begin of Use1");

    await next(context);

    Console.WriteLine("End of Use1");
});*/
//zastêpujeny powy¿sz¹ implementacjê z u¿yciem osobnej klasy
//klasa ta implementuje interfejs IMiddleware przez co musi byæ zarejestrowana w DI
app.UseMiddleware<Use1Middleware>();


//przekierowanie do nowego potoku na podstawie œcie¿ki (path) zapytania
app.Map("/hello", helloApp =>
{
    helloApp.Use(async (context, next) =>
    {
        Console.WriteLine("Begin of Hello Use");
        await next(context);
        Console.WriteLine("End of Hello Use");
    });

    /*helloApp.Run(async (context) =>
    {
        Console.WriteLine("Hello Run: Hello World!");
        await context.Response.WriteAsync("Hello World from /hello!");
    });*/
    helloApp.UseMiddleware<HelloRunMiddleware>();
});


/*app.Use(async (context, next) =>
{
    Console.WriteLine("Begin of Use2");
    await next(context);
    Console.WriteLine("End of Use2");
});*/
//zastêpujeny powy¿sz¹ implementacjê z u¿yciem osobnej klasy
//klasa ta nie implementuje interfejs IMiddleware przez co nie musi byæ zarejestrowana w DI
//musi jednak posiadaæ konstruktor przyjmuj¹cy RequestDelegate
//oraz metodê InvokeAsync przyjmuj¹c¹ HttpContext oraz zwracaj¹c¹ Task
app.UseMiddleware<Use2Middleware>();

//przekierowanie do nowego potoku na podstawie warunku (predicate) - w tym przyk³adzie sprawdzamy czy w zapytaniu jest parametr "name"
app.MapWhen(context => context.Request.Query.ContainsKey("name"), MapWhenApp()); // nie ma gotowego middleware dla MapWhen, wiêc definiujemy go jako metodê zwracaj¹c¹ Action<IApplicationBuilder>



//terminal middleware - koñczy przetwarzanie ¿¹dania
/*app.Run(async (context) =>
{
    Console.WriteLine("Run: Hello World!");
    await context.Response.WriteAsync("Hello World!");
});*/
app.UseMiddleware<RunMiddleware>();




//sposoby sprawdzania œrodowiska
if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => "Hello from development!");
}
else if (app.Environment.IsStaging())
{
    app.MapGet("/", () => "Hello from staging!");
}
else if (app.Environment.IsProduction())
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

static Action<IApplicationBuilder> MapWhenApp()
{
    return mappedApp =>
    {
        mappedApp.Run(async context =>
        {
            var name = context.Request.Query["name"];

            Console.WriteLine($"MapWhen: hello {name}");
            await context.Response.WriteAsync($"Hello {name}!");
        });
    };
}