using Core.Entities;
using DataModel.DTO;
using MicroServiceBase.Interfaces;
using MicroServiceBase.Options;
using MicroServiceBase.Utils;
using Microsoft.Extensions.Options;
using RemoteRESTClients.Interfaces;

namespace RemoteRESTClients.RESTClients
{
    public class ProductRemoteService : IProductRemoteService
    {
        private readonly IRestClient _restClient;
        private readonly IOptions<ServiceLocationOptions> _options;

        private string BaseUrl 
        {
            get
            {
                return _options.Value.Locations[ServiceNameHelper.ProductService];
            }     
        }

        public ProductRemoteService(IRestClient restClient, IOptions<ServiceLocationOptions> options)
        {
            _options = options;
            _restClient = restClient;
        }

        public async Task<ProductDTO> GetAsync(Guid uuid)
        {
            string url = $"{BaseUrl}/api/Product/Get?uuid={uuid}";
            return await _restClient.GetAsync<ProductDTO>(url);
        }

        public async Task<IReadOnlyCollection<ProductDTO>> GetAsync()
        {
            string url = $"{BaseUrl}/api/Product/Get";
            return await _restClient.GetAsync<List<ProductDTO>>(url);
        }

        Task<IReadOnlyCollection<ProductDTO>> IBaseRemoteService<ProductDTO>.FindAsync(Filter? filter, string? orderBy, bool? sortDescending)
        {
            throw new NotImplementedException();
        }
    }
}
