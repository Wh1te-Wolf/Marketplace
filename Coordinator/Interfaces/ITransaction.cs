using Coordinator.Entities;

namespace Coordinator.Interfaces
{
    public interface ITransaction
    {
        public Guid UUID { get; set; }
        public object TransactionData { get; set; }
        ICollection<TransactionLocalRequestInfo> GetProcessPrepareData();
        ICollection<TransactionLocalRequestInfo> GetProcessCommitData();
        Task ReleaseLocksAsync();
        Task RollbackAsync();
        Task SendPrepareEventsAsync();
    }
}
