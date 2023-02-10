using System.ComponentModel.DataAnnotations;
using TMP_API.Models;

namespace TMP_API.Entities;

public class OrderItem : BaseModel
{
    [Key]
    public int Id { get; set; }

    public Product Product { get; set; }

    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    public int? OrderId { get; set; }
}
