using TMP_API.Entities;

namespace TMP_API.Models.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public List<Order> Orders { get; set; }
    public DateTime DateAdded { get; set; }
}
