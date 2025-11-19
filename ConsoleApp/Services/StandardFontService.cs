namespace ConsoleApp.Services
{
    internal class StandardFontService : IFontService
    {
        public StandardFontService()
        {
            Console.WriteLine("StandardFontService constructor");
        }

        public string Render(string text)
        {
            return text;
        }
    }
}
