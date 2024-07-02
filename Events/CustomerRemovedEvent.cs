using Events.Data;
using Events.Entities;

namespace Events;

public class CustomerRemovedEvent : MarketplaceEventBase
{
    public CustomerRemovedEvent(Guid uuid) : base("Customer", "Removed", SetEventData(uuid))
    {

    }

    private static CustomerRemovedEventData SetEventData(Guid customerUUID)
    {
        return new CustomerRemovedEventData(customerUUID);
    }
}
