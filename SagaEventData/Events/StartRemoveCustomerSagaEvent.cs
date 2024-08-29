using Events.Entities;

namespace SagaEventData.Events
{
    public class StartRemoveCustomerSagaEvent : IMarketplaceEvent
    {
        public Guid UUID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public object EventData { get; set; }

        public StartRemoveCustomerSagaEvent()
        {
            Type = "CustomerRemoveSaga";
            SubType = "StartCustomerRemoveSaga";
            UUID = Guid.NewGuid();
        }
    }
}
