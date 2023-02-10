using TMP_API.Entities;

namespace TMP_API.Models.OrderItems
{
    public class CreateOrderItemDto
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
