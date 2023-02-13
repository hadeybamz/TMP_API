using System.ComponentModel.DataAnnotations;
using TMP_API.Entities;

namespace TMP_API.Models.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public List<int> OrderItems { get; set; }

        public string ShippingAddress { get; set; }
    }
}
