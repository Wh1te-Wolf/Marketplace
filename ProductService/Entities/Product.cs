using Core.Interfaces;

namespace ProductService.Entities
{
    public class Product : IEntity
    {
        public Guid UUID { get; set; }

        public string Name { get; set; }

        public List<ProductPrice> Prices { get; set; }

        public Guid CustomerUUID { get; set; }
    }
}
