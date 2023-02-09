using TMP_API.Models.Orders;

namespace TMP_API.Models.Customers.Dtos
{
    public class CustomerDto : BaseModel
    {
        public Int32 Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public List<Order> Orders { get; set; }
    }
}
