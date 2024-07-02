using Core.Interfaces;

namespace ProductCartService.Entities
{
    public class ProductCart : IEntity
    {
        public Guid UUID { get; set; }

        public ICollection<ProductCartItem> Items { get; set; } = null!;

        public Guid CustomerUUID { get; set; }
    }
}
