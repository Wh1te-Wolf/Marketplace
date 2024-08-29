using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using SagaEventData.EventData;
using SagaManager.Services.Interfaces;

namespace SagaManager.Handlers.CustomerRemoveSaga
{
    [MarketplaceEventHandler]
    public class CustomerRemoveRollbackHandler : IEventHandler
    {
        private readonly ISagaProcessManager _processManager;

        public CustomerRemoveRollbackHandler(ISagaProcessManager processManager)
        {
            _processManager = processManager;
        }

        public Task HandleAsync(IMarketplaceEvent marketplaceEvent)
        {
            CustomerSagaEventData sagaEventData = marketplaceEvent.EventData as CustomerSagaEventData;
            _processManager.AddSagaInformation(sagaEventData.TransactionUUID, false);
            return Task.CompletedTask;
        }
    }
}
