using Events;
using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using Events.Services.Interfaces;
using System.Reflection;
using TwoPhaseCommitUtils.Events;
using TwoPhaseCommitUtils.Services.Interfaces;

namespace TwoPhaseCommitUtils.Handlers
{
    [MarketplaceEventHandler]
    public class PrepareRequestHandler : IEventHandler
    {
        private readonly ILockManager _prepareManager;
        private readonly IEventManager _eventManager;

        public PrepareRequestHandler(ILockManager prepareManager, IEventManager eventManager)
        {
            _prepareManager = prepareManager;
            _eventManager = eventManager;
        }

        public async Task HandleAsync(IMarketplaceEvent marketplaceEvent)
        {
            PrepareEventData? eventData = marketplaceEvent.EventData as PrepareEventData;
            if (eventData is null)
                return;

            bool result = await _prepareManager.LockAsync(eventData.ObjectType, eventData.ObjectUUID);

            PrepareEventData prepareEventData = new PrepareEventData()
            {
                TransactionUUID = eventData.TransactionUUID,
                ObjectType = eventData.ObjectType,
                ObjectUUID = eventData.ObjectUUID,
                ServiceName = Assembly.GetEntryAssembly().FullName
            };

            string subType = result ? "Prepare.Success" : "Prepare.Failure";

            MarketplaceEventBase marketplaceEventBase = new MarketplaceEventBase("TwoPhaseCommit", subType, prepareEventData);
            await _eventManager.ProduceAsync(Topics.TwoPhaseCommitTopic, marketplaceEventBase);
        }
    }
}
