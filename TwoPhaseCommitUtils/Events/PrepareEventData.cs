namespace TwoPhaseCommitUtils.Events;

public class PrepareEventData
{
    public Guid TransactionUUID { get; set; }

    public string ObjectType { get; set; }

    public Guid ObjectUUID { get; set; }

    public string ServiceName { get; set; }
}
