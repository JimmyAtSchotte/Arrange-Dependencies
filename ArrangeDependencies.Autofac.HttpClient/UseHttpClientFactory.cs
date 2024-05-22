using System;
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
		
		public static IArrangeBuilder<ContainerBuilder> UseHttpClientFactory(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, Action<System.Net.Http.HttpClient> httpClientOptions, params HttpClientConfig[] configs)
		{
			if (arrangeBuilder is not ArrangeBuilder)
				return arrangeBuilder;

			var handler = CreateHandlerMock(configs);

			AddHttpClientFactory(arrangeBuilder, string.Empty, handler, httpClientOptions);

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
		
		public static IArrangeBuilder<ContainerBuilder> UseHttpClientFactory(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, string clientName, Action<System.Net.Http.HttpClient> httpClientOptions, params HttpClientConfig[] configs)
		{
			if (arrangeBuilder is not ArrangeBuilder)
				return arrangeBuilder;

			var handler = CreateHandlerMock(configs);

			AddHttpClientFactory(arrangeBuilder, clientName, handler, httpClientOptions);

			return arrangeBuilder;
		}

		private static void AddHttpClientFactory(IArrangeBuilder<ContainerBuilder> arrangeBuilder, string clientName,  Mock<HttpMessageHandler> handlerMock, Action<System.Net.Http.HttpClient> clientOptions = null)
		{
			var client = new System.Net.Http.HttpClient(handlerMock.Object);
			clientOptions?.Invoke(client);
			
			arrangeBuilder.UseMock<IHttpClientFactory>(mock =>
			{
				mock.Setup(x => x.CreateClient(clientName))
					.Returns(client);
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
					.ReturnsAsync(config.HttpResponseMessage)
					.Verifiable();
			}

			return handlerMock;
		}
	}
}