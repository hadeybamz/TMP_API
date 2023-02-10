using System.ComponentModel.DataAnnotations;

namespace TMP_API.Models.Customers;

public class CreateCustomerDto
{
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
}
