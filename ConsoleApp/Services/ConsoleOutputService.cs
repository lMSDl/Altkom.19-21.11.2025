namespace ConsoleApp.Services
{
    internal class ConsoleOutputService : BaseOutputService
    {
        public ConsoleOutputService(IFontService fontService) : base(fontService)
        {
            Console.WriteLine("ConsoleOutputService constructor");
        }

        protected override void PrintWithFont(string text)
        {
            Console.WriteLine(text);
        }
    }
}
