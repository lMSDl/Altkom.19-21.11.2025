using Microsoft.Extensions.Logging;

namespace ConsoleApp
{
    internal class LoggerDemo
    {
        private const string ScopeFormat = "Scope {0}";
        private readonly ILogger _logger;

        public LoggerDemo(ILogger<LoggerDemo> logger)
        {
            _logger = logger;
        }

        public void Work()
        {
            //_logger.LogInformation("LoggerDemo is starting work.");

            using (var scope1 = _logger.BeginScope(nameof(Work)))
                try
            {
                    using var scope3 = _logger.BeginScope(ScopeFormat, GetType().Name);
                    using var scope4 = _logger.BeginScope(new Dictionary<string, string> { { "a", "1" }, { "b", "2" } });

                    for (int i = 0; i < 10; i++)
                    {
                        //_logger.LogInformation("LoggerDemo is working. Iteration: {Iteration}", i);
                        using var scope2 = _logger.BeginScope($"Iteration {i}");

                        try
                        {
                            if (i == 5)
                                throw new InvalidOperationException("An error occurred during work.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "An exception occurred at iteration {Iteration}", i);
                        }

                        if (i == 9)
                            throw new ApplicationException("Critical failure in LoggerDemo.");
                    }
            }
            catch (Exception ex) when (LogError(ex))
            {
                //_logger.LogCritical(ex, "LoggerDemo encountered a critical error and will stop.");
            }
            finally
            {
                _logger.LogInformation("LoggerDemo has finished work.");
            }
        }

        private bool LogError(Exception ex)
        {
            _logger.LogCritical(ex, "LoggerDemo encountered a critical error and will stop.");
            return true;
        }
    }
}
