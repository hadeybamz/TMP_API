using TMP_API.Entities;
using TMP_API.Models.OrderItems;
using TMP_API.Models.Users;

namespace TMP_API.Models.Orders;

public class OrderDto
{
    public int Id { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public decimal OrderPrice { get; set; }
    public string ShippingAddress { get; set; }
    public String Customer { get; set; }
    public DateTime OrderDate { get; set; }
}
