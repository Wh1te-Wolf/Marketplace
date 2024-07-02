namespace ProductCartService.DTO
{
    public class BillItemDTO
    {
        public Guid UUID { get; set; }

        public Guid ProductUUID { get; set; }

        public int Quantity { get; set; }

        public Guid ProductPriceUUID { get; set; }

        public decimal Price { get; set; }
    }
}
