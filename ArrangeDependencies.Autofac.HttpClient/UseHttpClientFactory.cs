using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ArrangeDependencies.Autofac.Extensions;
using ArrangeDependencies.Core.Interfaces;
using Autofac;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace ArrangeDependencies.Autofac.HttpClient
{
    public static class UseHttpClientFactoryExtension
    {
        public static IArrangeBuilder<ContainerBuilder> UseHttpClientFactory(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, params HttpClientConfig[] configs)
        {
            if (!(arrangeBuilder is ArrangeBuilder builder)) 
                return arrangeBuilder;

            var handler = CreateHandlerMock(configs);
            
            AddHttpClientFactory(arrangeBuilder, string.Empty, handler);

            return arrangeBuilder;
        }
        
        public static IArrangeBuilder<ContainerBuilder> UseHttpClientFactory(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, string clientName, params HttpClientConfig[] configs)
        {
            if (!(arrangeBuilder is ArrangeBuilder builder)) 
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
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(string.Empty),
                })
                .Verifiable();

            foreach (var config in configs)
            {
                handlerMock.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        config.Expression,
                        ItExpr.IsAny<CancellationToken>()
                    )
                    .ReturnsAsync(new HttpResponseMessage()
                    {
                        StatusCode = config.HttpStatusCode,
                        Content = new StringContent(config.Content),
                    })
                    .Verifiable();
            }

            return handlerMock;
        }
    }

    public class HttpClientConfig
    {
        
        public System.Linq.Expressions.Expression Expression { get;  }
        public string Content { get; }
        public HttpStatusCode HttpStatusCode { get; }

        private HttpClientConfig(System.Linq.Expressions.Expression expression, string content, HttpStatusCode httpStatusCode)
        {
            Expression = expression;
            Content = content;
            HttpStatusCode = httpStatusCode;
        }
        
        public static HttpClientConfig Create(Uri uri, string content, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            return new HttpClientConfig(ItExpr.Is<HttpRequestMessage>(request => request.RequestUri == uri), content, httpStatusCode);
        }
        
        public static HttpClientConfig Create(Func<HttpRequestMessage, bool> expression, string content, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            return new HttpClientConfig(ItExpr.Is<HttpRequestMessage>(request => expression.Invoke(request)), content, httpStatusCode);
        }

        public static HttpClientConfig Create<T>(Uri uri, T content, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            return new HttpClientConfig(ItExpr.Is<HttpRequestMessage>(request => request.RequestUri == uri), JsonConvert.SerializeObject(content), httpStatusCode);
        }
        
        public static HttpClientConfig Create<T>(Func<HttpRequestMessage, bool> expression, T content, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            return new HttpClientConfig(ItExpr.Is<HttpRequestMessage>(request => expression.Invoke(request)), JsonConvert.SerializeObject(content), httpStatusCode);
        }
    }
}