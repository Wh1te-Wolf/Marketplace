using Events.Data.Interfaces;

namespace SagaManager.DTO
{
    public class StartSagaProcessDTO
    {
        public string Name { get; set; }

        public string TransactionEventDataString { get; set; }
    }
}
