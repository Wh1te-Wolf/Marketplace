using Events.Attributes;
using Events.Entities;
using Events.Handlers;
using Events.Services.Interfaces;
using TwoPhaseCommitUtils.Events;
using TwoPhaseCommitUtils.Services.Interfaces;

namespace TwoPhaseCommitUtils.Handlers
{
    [MarketplaceEventHandler]
    public class ReleaseLockHandler : IEventHandler
    {
        private readonly ILockManager _lockManager;

        public ReleaseLockHandler(ILockManager prepareManager)
        {
            _lockManager = prepareManager;
        }

        public async Task HandleAsync(IMarketplaceEvent marketplaceEvent)
        {
            ReleaseLockData releaseLockData = marketplaceEvent as ReleaseLockData;
            await _lockManager.ReleaseLockAsync(releaseLockData.Type, releaseLockData.UUID);
        }
    }
}
