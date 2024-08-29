namespace TwoPhaseCommitUtils.Entities;

public class LockData
{
    public Guid UUID { get; set; }

    public string Type { get; set; }

    public LockData(Guid uuid, string type)
    {
        UUID = uuid;
        Type = type;
    }
}
