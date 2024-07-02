using Core.Interfaces;

namespace ProductCartService.Entities
{
    public class ProductCartItem : IEntity
    {
        public Guid UUID { get; set; }

        public Guid ProductUUID { get; set; }

        public int Quantity { get; set; }

        public bool Included { get; set; }

        public Guid ProductCartUUID { get; set; }
    }
}
