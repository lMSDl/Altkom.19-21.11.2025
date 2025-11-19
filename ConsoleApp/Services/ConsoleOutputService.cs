using Microsoft.Extensions.Configuration;

namespace ConsoleApp.Services
{
    internal class ConsoleOutputService : BaseOutputService
    {

        //wstrzyknięcie IConfiguration
        public ConsoleOutputService(IFontService fontService, IConfiguration config) : base(fontService)
        {
            Console.WriteLine("ConsoleOutputService constructor");
            Console.WriteLine(config["HelloJson"]);
        }

        protected override void PrintWithFont(string text)
        {
            Console.WriteLine(text);
        }
    }
}
