namespace Events.Data
{
    public class CustomerRemovedEventData
    {
        public Guid CustomerUUID { get; set; }

        public CustomerRemovedEventData(Guid customerUUID)
        {
            CustomerUUID = customerUUID;
        }
    }
}
