namespace Coordinator.Entities.TransactionData
{
    public class CustomerRemoveTransactionData
    {
        public Guid CustomerUUID { get; set; }

        public Guid ProductCartUUID { get; set; }
    }
}
