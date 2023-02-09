using TMP_API.Models.Products;

namespace TMP_API.Models.Orders.Dtos;

public class OrderDto
{
    public Int32 Id { get; set; }
    public Int32 CustomerId { get; set; }
    public List<Product> Products { get; set; }
    public Decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
}
