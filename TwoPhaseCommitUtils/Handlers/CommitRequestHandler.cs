using Events;
using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using Events.Services.Interfaces;
using TwoPhaseCommitUtils.Events;
using TwoPhaseCommitUtils.Services.Interfaces;

namespace TwoPhaseCommitUtils.Handlers;

[MarketplaceEventHandler]
public class CommitRequestHandler : IEventHandler
{
    private readonly ICommitManager _commitManager;
    private readonly IEventManager _eventManager;

    public CommitRequestHandler(ICommitManager commitManager, IEventManager eventManager)
    {
        _commitManager = commitManager;
        _eventManager = eventManager;
    }

    public async Task HandleAsync(IMarketplaceEvent marketplaceEvent)
    {
        CommitEventData? eventData = marketplaceEvent.EventData as CommitEventData;
        if (eventData is null)
            return;

        bool result = await _commitManager.CommitAsync(eventData.TransactionUUID, eventData.EntityType, eventData.EntityCommitData);

        CommitEventData commitEventData = new CommitEventData()
        {
            TransactionUUID = eventData.TransactionUUID,
            EntityType = eventData.EntityType
        };

        string subType = result ? "Commit.Success" : "Commit.Failure";

        MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("TwoPhaseCommit", subType, commitEventData);
        await _eventManager.ProduceAsync(Topics.TwoPhaseCommitTopic, marketplaceEventBase);
    }
}
