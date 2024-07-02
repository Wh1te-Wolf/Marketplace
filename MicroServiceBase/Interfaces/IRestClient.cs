namespace MicroServiceBase.Interfaces
{
    public interface IRestClient
    {
        Task<string> GetAsync(string url, string? token = null, CancellationToken cancellationToken = default);

        Task<T> GetAsync<T>(string url, string? token = null, CancellationToken cancellationToken = default);

        Task<string> PostAsync(string url, string body, string? token = null, CancellationToken cancellationToken = default);

        Task<T> PostAsync<T>(string url, string body, string? token = null, CancellationToken cancellationToken = default);

        Task<string> PatchAsync(string url, string body, string? token = null, CancellationToken cancellationToken = default);

        Task<T> PatchAsync<T>(string url, string body, string? token = null, CancellationToken cancellationToken = default);

        Task<string> PutAsync(string url, string body, string? token = null, CancellationToken cancellationToken = default);

        Task<T> PutAsync<T>(string url, string body, string? token = null, CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> DeleteAsync(string url, string? token = null, CancellationToken cancellationToken = default);
    }
}
