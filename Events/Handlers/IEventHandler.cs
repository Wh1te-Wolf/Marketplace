using Events.Entities;

namespace Events.Handlers
{
    public interface IEventHandler
    {
        Task HandleAsync(IMarketplaceEvent marketplaceEvent);
    }
}
