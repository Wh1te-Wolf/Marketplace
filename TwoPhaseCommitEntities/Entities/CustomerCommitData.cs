using TwoPhaseCommitEntities.Entities.Intarfaces;

namespace TwoPhaseCommitEntities.Entities;

public class CustomerCommitData : IEntityCommitData
{
    public Guid UUID { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
}
