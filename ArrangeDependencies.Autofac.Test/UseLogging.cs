using ArrangeDependencies.Autofac.Logger;
using ArrangeDependencies.Autofac.Test.Basis.Interfaces;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class UseLogging
    {
        [Test]
        public void DependencyOnLoggerShouldVerifyLogError()
        {
            Mock<ILogger<LoggingService>> logger = null;

            var arrange = Arrange.Dependencies<ILoggingService, LoggingService>(dependencies =>
            {
                dependencies.UseLogger(out logger);
            });

            var loggingService = arrange.Resolve<ILoggingService>();
            loggingService.LogError();

            logger.Verify(LogLevel.Error, Times.Once());

        }

        [Test]
        public void DependencyOnLoggerFactoryShouldVerifyLogError()
        {
            Mock<ILogger<LoggingService>> logger = null;

            var arrange = Arrange.Dependencies<ILoggingService, LoggingFactoryService>(dependencies =>
            {
                dependencies.UseLogger(out logger);
            });

            var loggingService = arrange.Resolve<ILoggingService>();
            loggingService.LogError();

            logger.Verify(LogLevel.Error, Times.Once());

        }
    }
}
