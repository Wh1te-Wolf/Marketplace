using Core.Interfaces;
using System;
using TwoPhaseCommitEntities.Entities;
using TwoPhaseCommitEntities.Entities.Intarfaces;
using TwoPhaseCommitUtils.Services.Interfaces;
using UserService.Entities;

namespace UserService.Services.TwoPhaseCommit
{
    public class CommitDataProvider : ICommitDataProvider
    {
        public T GetCommitData<T>(IEntityCommitData entityCommitData) where T : class, IEntity
        {
            T? result = default(T);

            if (entityCommitData is CustomerCommitData customerCommitData)
            {
                result = new Customer()
                {
                    UUID = customerCommitData.UUID,
                    Name = customerCommitData.Name,
                    Surname = customerCommitData.Surname,
                    Email = customerCommitData.Email,
                    PhoneNumber = customerCommitData.PhoneNumber
                } as T;
            }

            return result;
        }
    }
}
