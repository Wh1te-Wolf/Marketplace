using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using SagaEventData.EventData;
using SagaManager.Services.Interfaces;

namespace SagaManager.Handlers.CustomerCreateSaga
{
    [MarketplaceEventHandler]
    public class CustomerCreatedRollbackHandler : IEventHandler
    {
        private readonly ISagaProcessManager _processManager;

        public CustomerCreatedRollbackHandler(ISagaProcessManager processManager)
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
