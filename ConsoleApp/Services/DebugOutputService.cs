using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Services
{
    internal class DebugOutputService : BaseOutputService
    {
        private readonly int _count;

        public DebugOutputService(IFontService fontService, IServiceProvider serviceProvider) : base(fontService)
        {
            Console.WriteLine("DebugOutputService constructor");
            //możemy wstrzyknąć IServiceProvider, aby pobrać dodatkowe zależności
            //pobranie parametru count z serwisu
            _count = serviceProvider.GetRequiredService<IConfiguration>().GetValue<int>("Count");
        }
        public DebugOutputService(IFontService fontService, int count) : base(fontService)
        {
            _count = count;
            Console.WriteLine("DebugOutputService constructor with count");
        }

        protected override void PrintWithFont(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
            for (int i = 1; i < _count; i++)
            {
                System.Diagnostics.Debug.WriteLine(text);
            }
        }
    }
}
