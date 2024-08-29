using Core.Interfaces;
using TwoPhaseCommitEntities.Entities.Intarfaces;

namespace TwoPhaseCommitUtils.Services.Interfaces;

public interface ICommitDataProvider 
{
    T GetCommitData<T>(IEntityCommitData entityCommitData) where T : class, IEntity;
}
