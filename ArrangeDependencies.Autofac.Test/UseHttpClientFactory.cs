using ArrangeDependencies.Autofac.HttpClient;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArrangeDependencies.Autofac.Test
{
	public class UseHttpClientFactory
	{
		[Test]
		public async Task DefaultNotFoundStatus()
		{
			var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependencies =>
			{
				dependencies.UseHttpClientFactory();
			});

			var httpClientService = arrange.Resolve<IHttpClientService>();
			var client = httpClientService.CreateClient();

			var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://localhost/"));
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
		}

		[TestCase("TEST")]
		[TestCase("Hello world")]
		public async Task Content(string content)
		{
			var uri = new Uri("https://localhost/");
			var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependencies =>
			{
				dependencies.UseHttpClientFactory(HttpClientConfig.Create(uri, content));
			});

			var httpClientService = arrange.Resolve<IHttpClientService>();
			var client = httpClientService.CreateClient();

			var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
			var result = await response.Content.ReadAsStringAsync();
			Assert.That(result, Is.EqualTo(content));
		}

		[TestCase("TEST")]
		[TestCase("Hello world")]
		public async Task NamedClient(string content)
		{
			const string clientName = "Test";
			var uri = new Uri("https://localhost/");
			var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependencies =>
			{
				dependencies.UseHttpClientFactory(clientName, HttpClientConfig.Create(uri, content));
			});

			var httpClientService = arrange.Resolve<IHttpClientService>();
			var client = httpClientService.CreateClient(clientName);

			var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
			var result = await response.Content.ReadAsStringAsync();
			Assert.That(result, Is.EqualTo(content));
		}

		[TestCase("https://localhost/")]
		[TestCase("https://google.com/")]
		public async Task ResponsePerUri(string url)
		{
			var uri = new Uri(url);
			var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependencies =>
			{
				dependencies.UseHttpClientFactory(HttpClientConfig.Create(uri, url));
			});

			var httpClientService = arrange.Resolve<IHttpClientService>();
			var client = httpClientService.CreateClient();

			var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
			var result = await response.Content.ReadAsStringAsync();
			Assert.That(result, Is.EqualTo(url));
		}

		[Test]
		public async Task JsonObject()
		{
			var testResponse = new TestResponse("Test response string");

			var uri = new Uri("https://localhost");
			var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependencies =>
			{
				dependencies.UseHttpClientFactory(HttpClientConfig.Create(uri, testResponse));
			});

			var httpClientService = arrange.Resolve<IHttpClientService>();
			var client = httpClientService.CreateClient();

			var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
			var result = JsonSerializer.Deserialize<TestResponse>(await response.Content.ReadAsStringAsync());
			Assert.That(result.Text, Is.EqualTo(testResponse.Text));
		}

		[Test]
		public async Task JsonObjectExpression()
		{
			var testResponse = new TestResponse("Test response string");

			var uri = new Uri("https://localhost");
			var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependencies =>
			{
				dependencies.UseHttpClientFactory(HttpClientConfig.Create(message => message.RequestUri == uri, testResponse));
			});

			var httpClientService = arrange.Resolve<IHttpClientService>();
			var client = httpClientService.CreateClient();

			var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
			var result = JsonSerializer.Deserialize<TestResponse>(await response.Content.ReadAsStringAsync());
			Assert.That(result.Text, Is.EqualTo(testResponse.Text));
		}

		[Test]
		public async Task ContentExpression()
		{
			var content = "Test response string";
			var uri = new Uri("https://localhost");
			var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependencies =>
			{
				dependencies.UseHttpClientFactory(HttpClientConfig.Create(message => message.RequestUri == uri, content));
			});

			var httpClientService = arrange.Resolve<IHttpClientService>();
			var client = httpClientService.CreateClient();

			var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
			var result = await response.Content.ReadAsStringAsync();
			Assert.That(result, Is.EqualTo(content));
		}

		[TestCase(HttpStatusCode.Unauthorized)]
		[TestCase(HttpStatusCode.InternalServerError)]
		[TestCase(HttpStatusCode.ServiceUnavailable)]
		public async Task StatusCode(HttpStatusCode statusCode)
		{
			var uri = new Uri("https://localhost");
			var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependencies =>
			{
				dependencies.UseHttpClientFactory(HttpClientConfig.Create(uri, string.Empty, statusCode));
			});

			var httpClientService = arrange.Resolve<IHttpClientService>();
			var client = httpClientService.CreateClient();

			var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
			Assert.That(response.StatusCode, Is.EqualTo(statusCode));
		}
		
		
		
		[Test]
		public async Task UsingBaseAddressOfHttpClient()
		{
			var content = "Test response string";
			var baseUri = new Uri("https://localhost/");
			var uri = new Uri(baseUri, "path");
			var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependencies =>
			{
				dependencies.UseHttpClientFactory(client => client.BaseAddress = baseUri, HttpClientConfig.Create(message => message.RequestUri == uri, content));
			});

			var httpClientService = arrange.Resolve<IHttpClientService>();
			var client = httpClientService.CreateClient();

			var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "path"));
			var result = await response.Content.ReadAsStringAsync();
			Assert.That(result, Is.EqualTo(content));
		}
		
		
		[Test]
		public async Task ChangeContentType()
		{
			var content = "Test response string";
			var baseUri = new Uri("https://localhost/");
			var uri1 = new Uri(baseUri, "path1");
			var uri2 = new Uri(baseUri, "path1");
			var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependencies =>
			{
				dependencies.UseHttpClientFactory(client => client.BaseAddress = baseUri, 
				HttpClientConfig.Create(
					message => message.RequestUri == uri1,
					response => response.Content = new StringContent(content, Encoding.UTF8, "text/plain"
					)));
				
				HttpClientConfig.Create(
				uri2,
				response => response.Content = new StringContent(content, Encoding.UTF8, "text/html"
				));
			});

			var httpClientService = arrange.Resolve<IHttpClientService>();
			var client = httpClientService.CreateClient();

			var response1 = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri1.PathAndQuery));
			var response2 = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri2.PathAndQuery));

			Assert.That(response1.Content.Headers.ContentType?.MediaType, Is.EqualTo("text/plain"));
			Assert.That(response2.Content.Headers.ContentType?.MediaType, Is.EqualTo("text/html"));
		}


		public class TestResponse
		{
			public string Text { get; set; }

			public TestResponse(string text)
			{
				Text = text;
			}
		}
	}
}