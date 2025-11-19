

using Microsoft.Extensions.Configuration;
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
    .Build();



for(int i = 0; i < int.Parse(config["Count"]); i++)
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


void Introduction() {

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