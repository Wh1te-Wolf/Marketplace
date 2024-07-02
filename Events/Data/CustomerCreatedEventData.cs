namespace Events.Data
{
    public class CustomerCreatedEventData
    {
        public Guid CustomerUUID { get; set; }

        public CustomerCreatedEventData(Guid customerUUID)
        {
            CustomerUUID = customerUUID;
        }
    }
}
