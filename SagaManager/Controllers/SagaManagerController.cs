using Events.Data.Interfaces;
using Events.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SagaManager.DTO;
using SagaManager.Services.Interfaces;

namespace SagaManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SagaManagerController : ControllerBase
{
    private readonly ISagaProcessManager _sagaProcessManager;
    private readonly JsonSerializerSettings _jsonSettings = new() { TypeNameHandling = TypeNameHandling.All };

    public SagaManagerController(ISagaProcessManager sagaProcessManager)
    {
        _sagaProcessManager = sagaProcessManager;
    }

    [HttpPost(nameof(ProcessSaga))]
    public async Task<IActionResult> ProcessSaga([FromBody] StartSagaProcessDTO startSagaProcessDTO)
    {
        try
        {
            ITransactionEventData transactionEventData = JsonConvert.DeserializeObject<ITransactionEventData>(startSagaProcessDTO.TransactionEventDataString, _jsonSettings);
            bool result = await _sagaProcessManager.ProcessAsync(startSagaProcessDTO.Name, transactionEventData);
            return Ok(result);
        }
        catch
        { 
            return Ok(false);
        }
    }
}
