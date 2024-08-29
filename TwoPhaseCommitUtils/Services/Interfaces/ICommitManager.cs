using Core.Interfaces;
using TwoPhaseCommitEntities.Entities.Intarfaces;
using TwoPhaseCommitUtils.Entities.Enums;

namespace TwoPhaseCommitUtils.Services.Interfaces;

public interface ICommitManager<T> where T : class, IEntity
{
    Task<bool> CommitAsync(Guid transactionUUID, IEntityCommitData entityCommitData, CUDAction action);
}
