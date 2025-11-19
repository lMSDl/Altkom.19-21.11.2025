

using ConsoleApp.Configurations.Models;
using ConsoleApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

//Microsoft.Extensions.Configuration
IConfiguration config = new ConfigurationBuilder()
    //wczytanie wielu plików konfiguracyjnych powoduje:
    //- nadpisanie wartości z wcześniejszych plików (jeśli klucz jest ten sam)
    //- scalenie wartości z różnych plików (jeśli klucze są różne)

    //Microsoft.Extensions.Configuration.Json
    .AddJsonFile("Configurations\\config.json", optional: true) //optional: true - plik może nie istnieć
                                                                //Microsoft.Extensions.Configuration.Xml
    .AddXmlFile("Configurations\\config.xml", optional: false, reloadOnChange: true) //reloadOnChange: true - automatyczne przeładowanie konfiguracji przy zmianie pliku bez restartu aplikacji
                                                                                     //Microsoft.Extensions.Configuration.Ini
    .AddIniFile("Configurations\\config.ini")
    //NetEscapades.Configuration.Yaml
    .AddYamlFile("Configurations\\config.yaml")

    //Microsoft.Extensions.Configuration.CommandLine
    //.AddCommandLine(args)
    //przykład mapowania skróconej nazwy przełącznika na klucz konfiguracyjny
    //przykład użycia: ConsoleApp.exe -c 5 --alamakota "kot ma ale"
    .AddCommandLine(args, new Dictionary<string, string> { { "-z1", "zmienna1" } })

    //Microsoft.Extensions.Configuration.EnvironmentVariables
    //domyślnie pobiera wszystkie zmienne środowiskowe
    //zmienne ładowane są tylko przy starcie hosta aplikacji (procesu)
    .AddEnvironmentVariables()

    .Build();

//obiekt do konfiguracji wstrzykiwania zależności
var serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

//rejestracja serwisów

serviceCollection.AddSingleton<IConfiguration>(config);


//transient - zawsze nowa instancja
if (DateTime.Now.Second % 2 == 0)
{
    //automatyczna rejestracja klasy na interfejs
    serviceCollection.AddTransient<IOutputService, DebugOutputService>();
}
else
{
    //manualna rejestracja klasy na interfejs z parametrem konstruktora
    serviceCollection.AddTransient<IOutputService>(localServiceProvider =>
                                                    //pobieramy wymagany serwis z kontenera
                                                    new DebugOutputService(localServiceProvider.GetRequiredService<IFontService>(),
                                                    //parameter count przekazany ręcznie
                                                    5));
}

//scoped - jedna instancja na scope (w aplikacji konsolowej scope działa jak singleton)
serviceCollection.AddScoped<IOutputService, ConsoleOutputService>();

//singleton - zawsze ta sama instancja
serviceCollection.AddSingleton<IFontService, SubZeroFontService>();
serviceCollection.AddSingleton<IFontService, StandardFontService>();


//zbudowanie dostawcy usług
var serviceProvider = serviceCollection.BuildServiceProvider();


//pobranie pojedynczej usługi - jeśli wiele usług pod tym samym interfejsem, powoduje wybranie tej ostatnio zarejestrowanej
var outputService = serviceProvider.GetRequiredService<IOutputService>();
outputService.Print("Hello from Output Service!");


//pobieramy kolekcję usług o tym samym zarejestrowanym interfejsie
foreach (var service in serviceProvider.GetServices<IOutputService>())
{
    service.Print("Hello from multiple Output Services!");
}



//symulacja scope - w aplikacji webowej scope jest tworzony na czas obsługi żądania
for (int i = 0; i < 3; i++)
{
    using (var scope = serviceProvider.CreateScope())
    {
        {
            outputService = scope.ServiceProvider.GetRequiredService<IOutputService>();
            outputService.Print($"Hello from scoped Output Service! Scope {i + 1}");
        }
        {
            outputService = scope.ServiceProvider.GetRequiredService<IOutputService>();
            outputService.Print($"Hello from scoped Output Service! Scope {i + 1} - second call");
        }    
    
    }

}


    Console.ReadLine();


void Introduction()
{

    //instrukcje najwyższego poziomu - top-level statements
    //pozwalają na pisanie kodu bez konieczności definiowania klasy i metody Main
    //wszystko w pliku z instrukcjami najwyższego poziomu jest traktowane jako ciało metody Main
    //tylko jeden plik w projekcie może zawierać instrukcje najwyższego poziomu
    Console.WriteLine("Hello, World!");
    DoSth();
    Nullable();

    //funckja w funkcji Main - nie może mieć modyfikatora dostępu
    static void DoSth()
    {
        System.Console.WriteLine("Doing something...");

    }

    static void Nullable()
    {
        int? a = null;
        //int b = null;

        string c = "a";
        string? d = null;

        ToUpper(c);
        ToUpper(d);

        string ToUpper(string s)
        {
            /*if(s is null)
                return string.Empty;*/
            return s.ToUpper();
        }

    }
}

static void Configuration(IConfiguration config)
{
    //for(int i = 0; i < int.Parse(config["Count"]); i++)
    //binder pozwala nam pobierać wartości z konfiguracji o wskazanym typie (nie tylko string)
    for (int i = 0; i < config.GetValue<int>("Count"); i++)
    {
        Console.WriteLine(config["HelloJson"]);
        Console.WriteLine(config["HelloXML"]);
        Console.WriteLine(config["HelloIni"]);
        //await Task.Delay(1000);
    }

    Console.WriteLine();

    //do zagnieżdżonych wartości można odwołać się poprzez dwukropek
    Console.WriteLine($"{config["Greetings:Value"]} from {config["Greetings:Target:From"]} to {config["Greetings:Target:To"]}");

    //lub pobrać sekcję i odwoływać się do niej poprzez indeksator
    var greetingsSection = config.GetSection("Greetings");
    Console.WriteLine($"{greetingsSection["Value"]} from {greetingsSection["Target:From"]} to {greetingsSection["Target:To"]}");

    //var targetSection = config.GetSection("Greetings:Target");
    var targetSection = greetingsSection.GetSection("Target");
    Console.WriteLine($"{greetingsSection["Value"]} from {targetSection["From"]} to {targetSection["To"]}");

    Console.WriteLine();

    Console.WriteLine(config["zmienna1"]);
    Console.WriteLine(config["zmienna2"]);
    Console.WriteLine(config["zmienna3"]);

    Console.WriteLine();

    Console.WriteLine(config["alamakota"]);


    //Microsoft.Extensions.Configuration.Binder
    //bindujemy konfigurację do wskazanego obiektu
    /*var greetings = new Greetings();
    //config.GetSection("Greetings").Bind(greetings);
    config.Bind("Greetings", greetings);*/

    //wytwarza obiekt i binduje konfigurację do wskazanego typu
    var greetings = config.GetSection("Greetings").Get<Greetings>();

    Console.WriteLine($"{greetings.Value} from {greetings.Target.From} to {greetings.Target.To}");
}