namespace Events.Options
{
    public class ActionFilterStack
    {
        public string EventType { get; set; }

        public string EventSubType { get; set; }

        public List<HandlerConfiguration> Handlers { get; set; }
    }
}
