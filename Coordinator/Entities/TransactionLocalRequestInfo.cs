namespace Coordinator.Entities
{
    public class TransactionLocalRequestInfo
    {
        public Guid TransactionUUID { get; set; }

        public TaskCompletionSource<bool> TaskCompletionSource { get; set; }

        public bool Result { get; set; } = false;

        /// <summary>
        /// Формат вида ServiceName.Entity
        /// </summary>
        public string Target { get; set; }

        public TransactionLocalRequestInfo(Guid transactionUUID, TaskCompletionSource<bool> taskCompletionSource, bool result = false)
        {
            TransactionUUID = transactionUUID;
            TaskCompletionSource = taskCompletionSource;
            Result = result;
        }

        public TransactionLocalRequestInfo()
        {
            
        }
    }
}
