using System.ComponentModel.DataAnnotations;
using TMP_API.Models.Products;

namespace TMP_API.Models.Orders;

public class Order : BaseModel
{
    public Order() => DateAdded = DateTime.Now;

    [Key]
    public Int32 Id { get; set; }
    [Required]
    public Int32 CustomerId { get; set; }
    [Required]
    public List<Product> Products { get; set; }
    [Required]
    public Decimal TotalPrice { get; set; }
}
