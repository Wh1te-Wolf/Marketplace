using Events.Data.Interfaces;

namespace RemoteRESTClients.Interfaces;

public interface ISagaRemoteProcessManager
{
    Task<bool> ProcessAsync(string sagaName, ITransactionEventData sagaData);
}
