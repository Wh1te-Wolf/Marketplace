using Events.Data.Interfaces;

namespace Events.Data
{
    public class SagaFinishedEventData : ITransactionEventData
    {
        public Guid TransactionUUID { get; set; }

        public bool Result { get; set; }
    }
}
