using Coordinator.Entities;
using Coordinator.Interfaces;
using Coordinator.Services.Interfaces;
using System.Collections.Concurrent;

namespace Coordinator.Services
{
    public class TransactionManager : ITransactionManager
    {
        private ConcurrentDictionary<Guid, ICollection<TransactionLocalRequestInfo>> _pendingPrepareResults;
        private ConcurrentDictionary<Guid, ICollection<TransactionLocalRequestInfo>> _pendingCommitResults;

        public void AddCommitResultInformation(string target, Guid transactionUUID, bool result)
        {
            if (!_pendingCommitResults.TryGetValue(transactionUUID, out ICollection<TransactionLocalRequestInfo>? requestsInfo))
                return;

            if (requestsInfo is null)
                return;
            
            TransactionLocalRequestInfo? transactionLocalRequestInfo = requestsInfo.FirstOrDefault(ri => ri.Target == target);

            if (transactionLocalRequestInfo is null)
                return;

            transactionLocalRequestInfo.Result = result;
            transactionLocalRequestInfo.TaskCompletionSource.SetResult(result);
        }

        public void AddPrepareResultInformation(string target, Guid transactionUUID, bool result)
        {
            if (!_pendingPrepareResults.TryGetValue(transactionUUID, out ICollection<TransactionLocalRequestInfo>? requestsInfo))
                return;

            if (requestsInfo is null)
                return;

            TransactionLocalRequestInfo? transactionLocalRequestInfo = requestsInfo.FirstOrDefault(ri => ri.Target == target);

            if (transactionLocalRequestInfo is null)
                return;

            transactionLocalRequestInfo.Result = result;
            transactionLocalRequestInfo.TaskCompletionSource.SetResult(result);
        }

        public async Task<bool> ProcessTransactionAsync(ITransaction transaction)
        {
            Guid transactionUUID = Guid.NewGuid();
            transaction.UUID = transactionUUID;

            ICollection<TransactionLocalRequestInfo> prepareData = transaction.GetProcessPrepareData();

            _pendingPrepareResults.TryAdd(transactionUUID, prepareData);
            await transaction.SendPrepareEventsAsync();
            await Task.WhenAll(prepareData.Select(pd => pd.TaskCompletionSource.Task));

            foreach (TransactionLocalRequestInfo data in _pendingPrepareResults[transactionUUID])
            {
                if (!data.Result)
                {
                    await transaction.ReleaseLocksAsync();
                    _pendingPrepareResults.TryRemove(transactionUUID, out _);
                    return false;
                }
            }

            ICollection<TransactionLocalRequestInfo> commitData = transaction.GetProcessPrepareData();
            _pendingCommitResults.TryAdd(transactionUUID, commitData);
            await Task.WhenAll(commitData.Select(pd => pd.TaskCompletionSource.Task));

            foreach (TransactionLocalRequestInfo data in _pendingCommitResults[transactionUUID])
            {
                if (!data.Result)
                {
                    await transaction.RollbackAsync();
                    await transaction.ReleaseLocksAsync();
                    _pendingPrepareResults.TryRemove(transactionUUID, out _);
                    return false;
                }
            }

            return true;
        }
    }
}
