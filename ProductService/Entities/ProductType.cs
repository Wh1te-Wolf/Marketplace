using Core.Interfaces;

namespace ProductService.Entities
{
    public class ProductType : IEntity
    {
        public Guid UUID { get; set; }

        public string Name { get; set; }
    }
}
