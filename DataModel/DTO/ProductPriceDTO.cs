using DataModel.Enum;

namespace DataModel.DTO;

public class ProductPriceDTO
{
    public Guid UUID { get; set; }

    public decimal Value { get; set; }

    public Currency Currency { get; set; }

    public DateTime DateTime { get; set; }

    public Guid ProductUUID { get; set; }

    public bool IsLastPrice { get; set; }
}
