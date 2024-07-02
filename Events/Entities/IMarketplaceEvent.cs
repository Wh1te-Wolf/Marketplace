namespace Events.Entities
{
    public interface IMarketplaceEvent
    {
        public Guid UUID { get; set; }

        public string Type { get; set; }

        public string SubType { get; set; }

        public object EventData { get; set; }
    }
}
