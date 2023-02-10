using TMP_API.Models.Orders;

namespace TMP_API.Models.Customers
{
    public class CustomerDto : BaseModel
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public List<OrderDto> Orders { get; set; }
    }
}
