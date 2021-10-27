using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ArrangeDependencies.Autofac.HttpClient;
using ArrangeDependencies.Autofac.Test.Basis.Entites;
using ArrangeDependencies.Autofac.Test.Basis.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ArrangeDependencies.Autofac.Test
{
    [TestFixture]
    public class UseHttpClientFactory
    {
        [Test]
        public async Task DefaultNotFoundStatus()
        {
            var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependecies =>
            {
                dependecies.UseHttpClientFactory();
            });

            var httpClientService = arrange.Resolve<IHttpClientService>();
            var client = httpClientService.CreateClient();

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://localhost/"));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [TestCase("TEST")]
        [TestCase("Hello world")]
        public async Task Content(string content)
        {
            var uri = new Uri("https://localhost/");
            var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependecies =>
            {
                dependecies.UseHttpClientFactory(HttpClientConfig.Create(uri, content));
            });

            var httpClientService = arrange.Resolve<IHttpClientService>();
            var client = httpClientService.CreateClient();

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(content, result);
        }
        
        [TestCase("TEST")]
        [TestCase("Hello world")]
        public async Task NamedClient(string content)
        {
            var clientName = "Test";
            var uri = new Uri("https://localhost/");
            var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependecies =>
            {
                dependecies.UseHttpClientFactory(clientName, HttpClientConfig.Create(uri, content));
            });

            var httpClientService = arrange.Resolve<IHttpClientService>();
            var client = httpClientService.CreateClient(clientName);

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(content, result);
        }
        
        [TestCase("https://localhost/")]
        [TestCase("https://google.com/")]
        public async Task ResponsePerUri(string url)
        {
            var uri = new Uri(url);
            var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependecies =>
            {
                dependecies.UseHttpClientFactory(HttpClientConfig.Create(uri, url));
            });

            var httpClientService = arrange.Resolve<IHttpClientService>();
            var client = httpClientService.CreateClient();

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(url, result);
        }
        
        [Test]
        public async Task JsonObject()
        {
            var testResponse = new TestResponse("Test response string");
          
            var uri = new Uri("https://localhost");
            var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependecies =>
            {
                dependecies.UseHttpClientFactory(HttpClientConfig.Create(uri, testResponse));
            });

            var httpClientService = arrange.Resolve<IHttpClientService>();
            var client = httpClientService.CreateClient();

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            var result = JsonConvert.DeserializeObject<TestResponse>(await response.Content.ReadAsStringAsync());
            Assert.AreEqual(testResponse.Text, result.Text);
        }
        
        [Test]
        public async Task JsonObjectExpression()
        {
            var testResponse = new TestResponse("Test response string");
          
            var uri = new Uri("https://localhost");
            var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependecies =>
            {
                dependecies.UseHttpClientFactory(HttpClientConfig.Create(message => message.RequestUri == uri, testResponse));
            });

            var httpClientService = arrange.Resolve<IHttpClientService>();
            var client = httpClientService.CreateClient();

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            var result = JsonConvert.DeserializeObject<TestResponse>(await response.Content.ReadAsStringAsync());
            Assert.AreEqual(testResponse.Text, result.Text);
        }
        
        [Test]
        public async Task ContentExpression()
        {
            var content = "Test response string";
            var uri = new Uri("https://localhost");
            var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependecies =>
            {
                dependecies.UseHttpClientFactory(HttpClientConfig.Create(message => message.RequestUri == uri, content));
            });

            var httpClientService = arrange.Resolve<IHttpClientService>();
            var client = httpClientService.CreateClient();

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            var result = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(content, result);
        }
        
        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        public async Task StatusCode(HttpStatusCode statusCode)
        {
            var uri = new Uri("https://localhost");
            var arrange = Arrange.Dependencies<IHttpClientService, HttpClientService>(dependecies =>
            {
                dependecies.UseHttpClientFactory(HttpClientConfig.Create(uri, string.Empty, statusCode));
            });

            var httpClientService = arrange.Resolve<IHttpClientService>();
            var client = httpClientService.CreateClient();

            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));
            Assert.AreEqual(statusCode, response.StatusCode);
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