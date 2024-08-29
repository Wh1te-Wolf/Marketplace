using Core.Interfaces;
using TwoPhaseCommitUtils.Entities.Enums;
using TwoPhaseCommitUtils.Services.Interfaces;

namespace UserService.Services.TwoPhaseCommit
{
    public class RollbackManager<T> : IRollbackManager<T> where T : class, IEntity
    {
        private record RollbackEntityData(T Entity, CUDAction Action);
        private Dictionary<Guid, List<RollbackEntityData>> _rollbackData = new ();

        public Task AddEntity(Guid transactionUUID, T entity, CUDAction action)
        {
            _rollbackData.Add(transactionUUID, new RollbackEntityData(entity, action));
            return Task.CompletedTask;
        }

        public Task<bool> RollbackAsync(Guid transactionUUID)
        {
            throw new NotImplementedException();
        }
    }
}
