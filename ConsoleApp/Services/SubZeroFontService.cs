namespace ConsoleApp.Services
{
    internal class SubZeroFontService : IFontService
    {
        public SubZeroFontService()
        {
            Console.WriteLine("SubZeroFontService constructor");
        }

        public string Render(string text)
        {
            return Figgle.FiggleFonts.SubZero.Render(text);
        }
    }
}
