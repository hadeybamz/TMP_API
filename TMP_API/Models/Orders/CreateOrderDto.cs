using System.ComponentModel.DataAnnotations;
using TMP_API.Entities;

namespace TMP_API.Models.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public List<OrderItem> OrderItems { get; set; }

        public string ShippingAddress { get; set; }

        public Guid UserId { get; set; }
    }
}
