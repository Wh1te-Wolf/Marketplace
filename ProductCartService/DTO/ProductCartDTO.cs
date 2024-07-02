using ProductCartService.Entities;

namespace ProductCartService.DTO
{
    public class ProductCartDTO
    {
        public Guid UUID { get; set; }

        public List<ProductCartItemDTO> Items { get; set; }

        public Guid CustomerUUID { get; set; }
    }
}
