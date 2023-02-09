namespace TMP_API.Models.Products.Dtos;

public class ProductDto : BaseModel
{
    public Int32 Id { get; set; }
    public String Name { get; set; }
    public Decimal Price { get; set; }
}
