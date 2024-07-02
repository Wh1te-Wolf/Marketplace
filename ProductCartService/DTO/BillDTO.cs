using ProductCartService.Entities.Enum;
using ProductCartService.Entities;

namespace ProductCartService.DTO
{
    public class BillDTO
    {
        public Guid UUID { get; set; }

        public Guid CustomerUUID { get; set; }

        public ICollection<BillItem> Items { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public decimal Price { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
