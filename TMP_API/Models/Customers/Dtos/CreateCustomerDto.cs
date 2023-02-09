using System.ComponentModel.DataAnnotations;

namespace TMP_API.Models.Customers.Dtos;

public class CreateCustomerDto
{
    [Required]
    [MaxLength(100)]
    public String FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public String LastName { get; set; }

    [Required]
    [MaxLength]
    public String Address { get; set; }
}
