using Events.Entities;

namespace SagaEventData.Events
{
    public class StartCreateCustomerSagaEvent : IMarketplaceEvent
    {
        public Guid UUID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public object EventData { get; set; }

        public StartCreateCustomerSagaEvent()
        {
            Type = "CustomerCreateSaga";
            SubType = "StartCustomerCreateSaga";
            UUID = Guid.NewGuid();
        }
    }
}
