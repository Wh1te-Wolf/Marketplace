namespace DataModel.DTO;

public class ProductDTO
{
    public Guid UUID { get; set; }

    public string Name { get; set; }

    public List<ProductPriceDTO> Prices { get; set; }

    public Guid CustomerUUID { get; set; }
}
