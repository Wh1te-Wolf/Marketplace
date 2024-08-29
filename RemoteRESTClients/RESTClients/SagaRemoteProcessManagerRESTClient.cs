using DataModel.DTO;
using Events.Data.Interfaces;
using Events.Entities;
using MicroServiceBase.Interfaces;
using MicroServiceBase.Options;
using MicroServiceBase.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RemoteRESTClients.DTO;
using RemoteRESTClients.Interfaces;
using System;

namespace RemoteRESTClients.RESTClients;

public class SagaRemoteProcessManagerRESTClient : ISagaRemoteProcessManager //TODO Сделать красивую сереализацию с указанием типов
{
    private readonly IRestClient _restClient;
    private readonly IOptions<ServiceLocationOptions> _options;
    private readonly JsonSerializerSettings _jsonSettings = new() { TypeNameHandling = TypeNameHandling.All };
    private string BaseUrl
    {
        get
        {
            return _options.Value.Locations[ServiceNameHelper.SagaManager];
        }
    }

    public SagaRemoteProcessManagerRESTClient(IRestClient restClient, IOptions<ServiceLocationOptions> options)
    {
        _options = options;
        _restClient = restClient;
    }

    public async Task<bool> ProcessAsync(string sagaName, ITransactionEventData sagaData)
    {
        string sagaDataString = JsonConvert.SerializeObject(sagaData, _jsonSettings);
        StartSagaProcessDTO startSagaProcessDTO = new StartSagaProcessDTO()
        {
            Name = sagaName,
            TransactionEventDataString = sagaDataString
        };

        string request = JsonConvert.SerializeObject(startSagaProcessDTO);

        string url = $"{BaseUrl}/api/SagaManager/ProcessSaga";
        bool result = await _restClient.PostAsync<bool>(url, request);
        return result;
    }
}
