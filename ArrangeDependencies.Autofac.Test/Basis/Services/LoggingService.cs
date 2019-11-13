using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using Microsoft.Extensions.Logging;

namespace ArrangeDependencies.Autofac.Test.Basis.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogError()
        {
            _logger.LogError("Error");
        }
    }

    public class LoggingFactoryService : ILoggingService
    {
        private readonly ILogger _logger;

        public LoggingFactoryService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(LoggingFactoryService));
        }

        public void LogError()
        {
            _logger.LogError("Error");
        }
    }
}
