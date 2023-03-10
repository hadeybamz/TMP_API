using TMP_API.Models.Products;

namespace TMP_API.Models.OrderItems
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateAdded { get; set; }
        public int? OrderId { get; set; }
        public string User { get; set; }
    }
}
