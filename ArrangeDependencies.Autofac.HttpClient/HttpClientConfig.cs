using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using Moq.Protected;
using Newtonsoft.Json;

namespace ArrangeDependencies.Autofac.HttpClient
{
	public class HttpClientConfig
	{
		public Expression Expression { get; }
		public string Content { get; }
		public HttpStatusCode HttpStatusCode { get; }

		private HttpClientConfig(Expression expression, string content, HttpStatusCode httpStatusCode)
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