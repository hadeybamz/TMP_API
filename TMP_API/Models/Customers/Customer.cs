using System.ComponentModel.DataAnnotations;
using TMP_API.Models.Orders;

namespace TMP_API.Models.Customers
{
    public class Customer : BaseModel
    {
        public Customer()
        {
            DateAdded = DateTime.Now;
        }
        [Key]
        public Int32 Id { get; set; }

        [Required]
        [MaxLength(100)]
        public String FirstName { get; set; }
        
        [Required]
        [MaxLength (100)]
        public String LastName { get; set; }
        
        [Required]
        [MaxLength]
        public String Address { get; set; }

        public List<Order> Orders { get; set; }
    }
}
