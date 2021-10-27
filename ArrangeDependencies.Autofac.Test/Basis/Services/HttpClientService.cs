using System.Net.Http;

namespace ArrangeDependencies.Autofac.Test.Basis.Services
{
    public interface IHttpClientService
    {
        System.Net.Http.HttpClient CreateClient();
        System.Net.Http.HttpClient CreateClient(string name);
    }

    public class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpClientService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public System.Net.Http.HttpClient CreateClient()
        {
            return _clientFactory.CreateClient();
        }

        public System.Net.Http.HttpClient CreateClient(string name)
        {
            return _clientFactory.CreateClient(name);
        }
    }
}