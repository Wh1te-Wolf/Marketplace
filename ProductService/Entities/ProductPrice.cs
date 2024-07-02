using Core.Interfaces;
using DataModel.Enum;

namespace ProductService.Entities
{
    public class ProductPrice : IEntity
    {
        public Guid UUID { get; set; }

        public decimal Value { get; set; }

        public Currency Currency { get; set; }

        public DateTime DateTime { get; set; }

        public Guid ProductUUID { get; set;}

        public Product Product { get; set; }

        public bool IsLastPrice { get; set; }
    }
}
