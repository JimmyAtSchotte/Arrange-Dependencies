using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Core.Interfaces;
using Autofac;
using Moq;
using Moq.Protected;

namespace ArrangeDependencies.Autofac.HttpClient
{
	public static class UseHttpClientFactoryExtension
	{
		public static IArrangeBuilder<ContainerBuilder> UseHttpClientFactory(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, params HttpClientConfig[] configs)
		{
			if (arrangeBuilder is not ArrangeBuilder)
				return arrangeBuilder;

			var handler = CreateHandlerMock(configs);

			AddHttpClientFactory(arrangeBuilder, string.Empty, handler);

			return arrangeBuilder;
		}

		public static IArrangeBuilder<ContainerBuilder> UseHttpClientFactory(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, string clientName, params HttpClientConfig[] configs)
		{
			if (arrangeBuilder is not ArrangeBuilder)
				return arrangeBuilder;

			var handler = CreateHandlerMock(configs);

			AddHttpClientFactory(arrangeBuilder, clientName, handler);

			return arrangeBuilder;
		}

		private static void AddHttpClientFactory(IArrangeBuilder<ContainerBuilder> arrangeBuilder, string clientName, Mock<HttpMessageHandler> handlerMock)
		{
			arrangeBuilder.UseMock<IHttpClientFactory>(mock =>
			{
				mock.Setup(x => x.CreateClient(clientName))
					.Returns(new System.Net.Http.HttpClient(handlerMock.Object));
			});
		}

		private static Mock<HttpMessageHandler> CreateHandlerMock(IEnumerable<HttpClientConfig> configs)
		{
			var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			handlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.NotFound,
					Content = new StringContent(string.Empty),
				})
				.Verifiable();

			foreach (var config in configs)
			{
				handlerMock.Protected()
					.Setup<Task<HttpResponseMessage>>("SendAsync", config.Expression, ItExpr.IsAny<CancellationToken>())
					.ReturnsAsync(new HttpResponseMessage
					{
						StatusCode = config.HttpStatusCode,
						Content = new StringContent(config.Content),
					})
					.Verifiable();
			}

			return handlerMock;
		}
	}
}