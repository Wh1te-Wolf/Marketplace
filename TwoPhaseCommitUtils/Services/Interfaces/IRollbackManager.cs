using Core.Interfaces;
using TwoPhaseCommitUtils.Entities.Enums;

namespace TwoPhaseCommitUtils.Services.Interfaces
{
    public interface IRollbackManager<T> where T : class, IEntity
    {
        Task<bool> RollbackAsync(Guid transactionUUID);

        Task AddEntity(Guid transactionUUID, T entity, CUDAction action);
    }
}
