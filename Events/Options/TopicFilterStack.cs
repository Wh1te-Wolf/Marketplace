namespace Events.Options
{
    public class TopicFilterStack
    {
        public string Topic { get; set; }

        public List<ActionFilterStack> Actions { get; set; }
    }
}
