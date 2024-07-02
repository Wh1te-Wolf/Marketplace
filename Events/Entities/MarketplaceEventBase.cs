namespace Events.Entities
{
    public abstract class MarketplaceEventBase : IMarketplaceEvent
    {
        public Guid UUID { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public object EventData { get; set; }

        protected MarketplaceEventBase(string type, string subtype, object data)
        {
            Type = type;
            SubType = subtype;
            EventData = data;
            UUID = Guid.NewGuid();
        }
    }
}
