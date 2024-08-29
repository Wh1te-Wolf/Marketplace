using Coordinator.Interfaces;

namespace Coordinator.Services.Interfaces
{
    public interface ITransactionManager
    {
        Task<bool> ProcessTransactionAsync(ITransaction transaction);

        void AddPrepareResultInformation(string target, Guid transactionUUID, bool result);

        void AddCommitResultInformation(string target, Guid transactionUUID, bool result);
    }
}
