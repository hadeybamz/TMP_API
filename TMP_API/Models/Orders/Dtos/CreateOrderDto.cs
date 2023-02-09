using System.ComponentModel.DataAnnotations;
using TMP_API.Models.Products;

namespace TMP_API.Models.Orders.Dtos
{
    public class CreateOrderDto
    {
        [Required]
        public Int32 CustomerId { get; set; }
        [Required]
        public List<Product> Products { get; set; }
    }
}
