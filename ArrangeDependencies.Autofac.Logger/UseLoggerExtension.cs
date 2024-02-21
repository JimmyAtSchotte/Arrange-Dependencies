using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Core.Interfaces;
using Autofac;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Language.Flow;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ArrangeDependencies.Autofac.Logger
{
	public static class UseLoggerExtension
	{
		/// <summary>
		/// Creates a Mock as an out parameter that you can verify logging on LogLevel
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arrangeBuilder"></param>
		/// <param name="logger"></param>
		public static IArrangeBuilder<ContainerBuilder> UseLogger<T>(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, out Mock<ILogger<T>> logger)
		{
			if (arrangeBuilder is ArrangeBuilder builder)
				logger = AddLogger<T>(builder);
			else
				logger = new Mock<ILogger<T>>();

			return arrangeBuilder;
		}

		private static Mock<ILogger<T>> AddLogger<T>(ArrangeBuilder arrangeBuilder)
		{
			arrangeBuilder.UseMock<ILogger<T>>(mock =>
			{
				foreach (var logLevel in Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>())
					mock.SetupLogLevel(logLevel);

			}, out var logger);

			arrangeBuilder.UseMock<ILoggerFactory>(mock =>
			{
				mock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
			});

			return logger;
		}

		private static ISetup<ILogger<T>> SetupLogLevel<T>(this Mock<ILogger<T>> logger, LogLevel level)
		{
			return logger.Setup(x => x.Log(level, It.IsAny<EventId>(), It.IsAny<T>(), It.IsAny<Exception>(), It.IsAny<Func<T, Exception, string>>()));
		}

		private static Expression<Action<ILogger<T>>> Verify<T>(LogLevel level)
		{
			return x => x.Log(level,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((o, t) => true),
					It.IsAny<Exception>(),
					(Func<It.IsAnyType, Exception, string>)It.IsAny<object>());
		}

		public static void Verify<T>(this Mock<ILogger<T>> mock, LogLevel level, Times times)
		{
			mock.Verify(Verify<T>(level), times);
		}
	}
}