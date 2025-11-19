namespace ConsoleApp.Services
{
    internal abstract class BaseOutputService : IOutputService
    {
        private readonly IFontService _fontService;
        protected BaseOutputService(IFontService fontService)
        {
            _fontService = fontService;
        }

        public void Print(string text)
        {
            PrintWithFont(_fontService.Render(text));
        }

        protected abstract void PrintWithFont(string text);

    }
}
