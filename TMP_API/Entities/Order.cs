using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TMP_API.Models;
using TMP_API.Models.Users;

namespace TMP_API.Entities;

public class Order : BaseModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public List<OrderItem> OrderItems { get; set; }

    public string ShippingAddress { get; set; }

    public Decimal TotalPrice { get; set; }
}
