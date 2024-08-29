using Core.Interfaces;
using TwoPhaseCommitEntities.Entities.Intarfaces;
using TwoPhaseCommitUtils.Entities.Enums;
using TwoPhaseCommitUtils.Services.Interfaces;

namespace UserService.Services.TwoPhaseCommit
{
    public class CommitManager<T> : ICommitManager<T> where T : class, IEntity
    {
        private readonly ICommitDataProvider _commitDataProvider;
        private readonly IBaseRepository<T> _baseRepository;

        public CommitManager(ICommitDataProvider commitDataProvider, IBaseRepository<T> baseRepository)
        {
            _commitDataProvider = commitDataProvider;
            _baseRepository = baseRepository;
        }

        public async Task<bool> CommitAsync(Guid transactionUUID, IEntityCommitData entityCommitData, CUDAction action)
        {
            try
            {
                T entity = _commitDataProvider.GetCommitData<T>(entityCommitData);

                switch (action)
                {
                    case CUDAction.Create:
                        await _baseRepository.AddAsync(entity);
                        break;

                    case CUDAction.Delete:
                        await _baseRepository.DeleteAsync(entity.UUID);
                        break;

                    case CUDAction.Update:
                        await _baseRepository.UpdateAsync(entity);
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
