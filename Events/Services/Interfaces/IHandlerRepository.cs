using Events.Entities;

namespace Events.Services.Interfaces;

public interface IHandlerRepository
{
    Task HandleAsync(string handler, IMarketplaceEvent marketplaceEvent);

    Task Initialize();
}
