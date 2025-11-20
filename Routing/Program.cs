var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//routing odpowiada za odnajsywanie endpointów
//app.UseRouting();

app.Use(async (context, next) =>
{
    Console.WriteLine("1 " + context.GetEndpoint()?.DisplayName);
    await next(context);
});

//jawne wywo³anie routingu powoduje, ¿e dzia³a on od miejsca wywo³ania, a nie od pocz¹tku potoku (domyœlna implementacja)
app.UseRouting();

app.Use(async (context, next) =>
{
    Console.WriteLine("2 " + context.GetEndpoint()?.DisplayName);
    await next(context);
});


//top level route handler - minimal API
//"metoda skrótu" do definiowania endpointów
//³amie zasadê kolejnoœci, poniewa¿ jest przepisywana do UseEndpoints
app.MapGet("/", () => "Hello World!");


//middleware obs³uguj¹cy endpointy
//jeœli go nie jawnie to jest dodawany automatycznie na koñcu potoku
/*app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});*/

//middleware terminuj¹cy nie pozwalaj¹ na obs³ugê przez endpointy jeœli wystêpuj¹ wczeœniej w potoku
app.Run(async context =>
{
    await context.Response.WriteAsync("DEAD END");
});

app.Run();
