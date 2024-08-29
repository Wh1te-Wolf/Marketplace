using Coordinator.Services.Interfaces;
using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using TwoPhaseCommitUtils.Events;

namespace Coordinator.Handlers
{
    [MarketplaceEventHandler]
    public class PrepareFailureResponseHandler : IEventHandler
    {
        private readonly ITransactionManager _transactionManager;

        public PrepareFailureResponseHandler(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public Task HandleAsync(IMarketplaceEvent marketplaceEvent)
        {
            PrepareEventData prepareEventData = marketplaceEvent.EventData as PrepareEventData;
            string target = $"{prepareEventData.ServiceName}.{prepareEventData.ObjectType}";
            _transactionManager.AddPrepareResultInformation(target, prepareEventData.TransactionUUID, false);
            return Task.CompletedTask;
        }
    }
}
