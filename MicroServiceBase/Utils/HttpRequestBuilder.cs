using System.Net.Http.Headers;
using System.Net.Mime;

namespace MicroServiceBase.Utils
{
    public class HttpRequestBuilder
    {
        private HttpMethod? _method;

        private string _url = string.Empty;

        private string? _token = null;

        private HttpContent? _content;

        private const string _defaultAcceptHeader = MediaTypeNames.Application.Json;

        private string? _acceptHeader = null;

        public HttpRequestBuilder AddMethod(HttpMethod httpMethod)
        { 
            _method = httpMethod;
            return this;
        }

        public HttpRequestBuilder AddUrl(string url)
        {
            _url = url;
            return this;
        }

        public HttpRequestBuilder AddToken(string token)
        {
            _token = token;
            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            _content = content;
            return this;
        }

        public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
        {
            _acceptHeader = acceptHeader;
            return this;
        }

        public HttpRequestMessage Build()
        { 
            if(_method is null)
                throw new ArgumentNullException("method");

            if (string.IsNullOrEmpty(_url))
                throw new ArgumentNullException("Url");

            if (!Uri.TryCreate(_url, UriKind.Absolute, out _))
                throw new ArgumentNullException("Url");

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage() 
            { 
                Method = _method,
                RequestUri = new Uri(_url),
            };

            if (_content is not null)
                httpRequestMessage.Content = _content;

            if (string.IsNullOrEmpty(_token))
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            if (string.IsNullOrEmpty(_acceptHeader))
                _acceptHeader = _defaultAcceptHeader;

            httpRequestMessage.Headers.Accept.Clear();

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_acceptHeader));

            return httpRequestMessage;
        }
    }
}
