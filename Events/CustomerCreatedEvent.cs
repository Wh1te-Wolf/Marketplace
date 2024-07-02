using Events.Data;
using Events.Entities;

namespace Events;

public class CustomerCreatedEvent : MarketplaceEventBase
{
    public CustomerCreatedEvent(Guid uuid) : base("Customer", "Created", SetEventData(uuid))
    {

    }

    private static CustomerCreatedEventData SetEventData(Guid customerUUID)
    {
        return new CustomerCreatedEventData(customerUUID);
    }
}
