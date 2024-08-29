using Coordinator.Services.Interfaces;
using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using TwoPhaseCommitUtils.Events;

namespace Coordinator.Handlers
{
    [MarketplaceEventHandler]
    public class CommitFailureResponseHandler : IEventHandler
    {
        private readonly ITransactionManager _transactionManager;

        public CommitFailureResponseHandler(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public Task HandleAsync(IMarketplaceEvent marketplaceEvent)
        {
            CommitEventData commitEventData = marketplaceEvent.EventData as CommitEventData;
            string target = $"{commitEventData.ServiceName}.{commitEventData.EntityType}";
            _transactionManager.AddCommitResultInformation(target, commitEventData.TransactionUUID, false);
            return Task.CompletedTask;
        }
    }
}
