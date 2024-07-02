using Core.Interfaces;

namespace ProductCartService.Entities
{
    public class BillItem : IEntity
    {
        public Guid UUID { get; set; }

        public Guid ProductUUID { get; set; }

        public int Quantity { get; set; }

        public Guid ProductPriceUUID { get; set; }

        public decimal Price { get; set; }

        public Guid BillUUID { get; set; }
    }
}
