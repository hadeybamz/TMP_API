using System.ComponentModel.DataAnnotations;
using TMP_API.Models;

namespace TMP_API.Entities;

public class Product : BaseModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
}


