using MicroServiceBase.Interfaces;
using MicroServiceBase.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mime;
using System.Text;

namespace MicroServiceBase.Services
{
    public class RestClient : IRestClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RestClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private readonly string _httpClientName = nameof(RestClient);
        private const string _jsonMediaType = MediaTypeNames.Application.Json;
        private static readonly Encoding _jsonEncoding = Encoding.UTF8;

        protected string HttpClientName
        {
            get => _httpClientName;
            init => _httpClientName = string.IsNullOrEmpty(value) ? nameof(RestClient) : value;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, string? token, CancellationToken cancellationToken = default)
        {
            using HttpClient client = _httpClientFactory.CreateClient(HttpClientName);
            using HttpRequestMessage httpRequestMessage = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Delete)
                .AddUrl(url)
                .AddAcceptHeader(_jsonMediaType)
                .AddToken(token)
                .Build();

            using HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);

            return httpResponseMessage;
        }

        public async Task<string> GetAsync(string url, string? token, CancellationToken cancellationToken = default)
        {
            using HttpClient client = _httpClientFactory.CreateClient(HttpClientName);
            using HttpRequestMessage httpRequestMessage = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddUrl(url)
                .AddAcceptHeader(_jsonMediaType)
                .AddToken(token)
                .Build();

            using HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception();

            string content = await httpResponseMessage.Content.ReadAsStringAsync();
            
            return content;
        }

        public async Task<T> GetAsync<T>(string url, string? token, CancellationToken cancellationToken = default)
        {
            string response = await GetAsync(url, token, cancellationToken);
            T result = JsonConvert.DeserializeObject<T>(response);
            return result;
        }

        public async Task<string> PatchAsync(string url, string body, string? token, CancellationToken cancellationToken = default)
        {
            using HttpClient client = _httpClientFactory.CreateClient(HttpClientName);
            using HttpRequestMessage httpRequestMessage = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Patch)
                .AddUrl(url)
                .AddAcceptHeader(_jsonMediaType)
                .AddToken(token)
                .AddContent(new StringContent(body, _jsonEncoding, _jsonMediaType))
                .Build();

            using HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception();

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            return content;
        }

        public async Task<T> PatchAsync<T>(string url, string body, string? token, CancellationToken cancellationToken = default)
        {
            string response = await PatchAsync(url, body, token, cancellationToken);
            T result = JsonConvert.DeserializeObject<T>(response);
            return result;
        }

        public async Task<string> PostAsync(string url, string body, string? token, CancellationToken cancellationToken = default)
        {
            using HttpClient client = _httpClientFactory.CreateClient(HttpClientName);
            using HttpRequestMessage httpRequestMessage = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Post)
                .AddUrl(url)
                .AddAcceptHeader(_jsonMediaType)
                .AddToken(token)
                .AddContent(new StringContent(body, _jsonEncoding, _jsonMediaType))
                .Build();

            using HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception();

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            return content;
        }

        public async Task<T> PostAsync<T>(string url, string body, string? token, CancellationToken cancellationToken = default)
        {
            string response = await PostAsync(url, body, token, cancellationToken);
            T result = JsonConvert.DeserializeObject<T>(response);
            return result;
        }

        public async Task<string> PutAsync(string url, string body, string? token, CancellationToken cancellationToken = default)
        {
            using HttpClient client = _httpClientFactory.CreateClient(HttpClientName);
            using HttpRequestMessage httpRequestMessage = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Put)
                .AddUrl(url)
                .AddAcceptHeader(_jsonMediaType)
                .AddToken(token)
                .AddContent(new StringContent(body, _jsonEncoding, _jsonMediaType))
                .Build();

            using HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception();

            string content = await httpResponseMessage.Content.ReadAsStringAsync();

            return content;
        }

        public async Task<T> PutAsync<T>(string url, string body, string? token, CancellationToken cancellationToken = default)
        {
            string response = await PutAsync(url, body, token, cancellationToken);
            T result = JsonConvert.DeserializeObject<T>(response);
            return result;
        }
    }
}
