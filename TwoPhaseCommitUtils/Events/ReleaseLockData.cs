namespace TwoPhaseCommitUtils.Events
{
    public class ReleaseLockData
    {
        public Guid UUID { get; set; }

        public string Type { get; set; }
    }
}
