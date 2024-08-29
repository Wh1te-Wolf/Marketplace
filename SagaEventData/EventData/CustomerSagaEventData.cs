using Events.Data.Interfaces;

namespace SagaEventData.EventData;

public class CustomerSagaEventData : ITransactionEventData
{
    public Guid UUID { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
    public Guid TransactionUUID { get; set; }

    public CustomerSagaEventData(Guid uuid)
    {
        UUID = uuid;
    }

    public CustomerSagaEventData()
    {
        
    }
}
