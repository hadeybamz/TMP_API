using System.ComponentModel.DataAnnotations;
using TMP_API.Models;

namespace TMP_API.Entities;

public class Customer : BaseModel
{
    [Key]
    public int CustomerId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(100)]
    public string EmailAddress { get; set; }

    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }

    [Required]
    [MaxLength]
    public string Address { get; set; }

    public List<Order> Orders { get; set; }
}
