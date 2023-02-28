using System.ComponentModel.DataAnnotations;
using TMP_API.Models;

namespace TMP_API.Entities;

public class Order : BaseModel
{
    [Key]
    public int Id { get; set; }

    public List<OrderItem> OrderItems { get; set; }

    public string ShippingAddress { get; set; }
}
