using TwoPhaseCommitEntities.Entities.Intarfaces;
using TwoPhaseCommitUtils.Entities.Enums;

namespace TwoPhaseCommitUtils.Events;

public class CommitEventData
{
    public Guid TransactionUUID { get; set; }

    public CUDAction Action { get; set; }

    public IEntityCommitData EntityCommitData { get; set; }

    public string EntityType { get; set; }

    public string ServiceName { get; set; }
}
