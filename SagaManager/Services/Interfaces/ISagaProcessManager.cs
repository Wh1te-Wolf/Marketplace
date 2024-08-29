using Events.Data.Interfaces;
using Events.Entities;

namespace SagaManager.Services.Interfaces
{
    public interface ISagaProcessManager
    {
        Task<bool> ProcessAsync(string sagaName, ITransactionEventData sagaData);

        public void AddSagaInformation(Guid transactionUUID, bool result);

        public void AddStartingEvent(string sagaName, Type type);
    }
}
