using TwoPhaseCommitEntities.Entities.Intarfaces;

namespace TwoPhaseCommitEntities.Entities;

public class ProductCartCommitData : IEntityCommitData
{
    public Guid UUID { get; set; }

    public string Name { get; set; }
}
