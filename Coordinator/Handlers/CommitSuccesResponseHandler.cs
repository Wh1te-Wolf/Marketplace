using Coordinator.Services.Interfaces;
using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using TwoPhaseCommitUtils.Events;

namespace Coordinator.Handlers
{
    [MarketplaceEventHandler]
    public class CommitSuccesResponseHandler : IEventHandler
    {
        private readonly ITransactionManager _transactionManager;

        public CommitSuccesResponseHandler(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public Task HandleAsync(IMarketplaceEvent marketplaceEvent)
        {
            CommitEventData commitEventData = marketplaceEvent.EventData as CommitEventData;
            string target = $"{commitEventData.ServiceName}.{commitEventData.EntityType}";
            _transactionManager.AddCommitResultInformation(target, commitEventData.TransactionUUID, true);
            return Task.CompletedTask;
        }
    }
}
