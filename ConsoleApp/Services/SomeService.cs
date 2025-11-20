using Microsoft.Extensions.Logging;

namespace ConsoleApp.Services
{
    internal class SomeService
    {
        private readonly ILogger _logger;
        public SomeService(ILogger<SomeService> logger)
        {
            _logger = logger;
            _logger.LogTrace("SomeService constructor");
        }

        public void DoWork()
        {
            _logger.LogInformation("SomeService is doing work.");
            // Implement some work logic here
        }

    }
}
