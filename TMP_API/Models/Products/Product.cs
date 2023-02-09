using System.ComponentModel.DataAnnotations;

namespace TMP_API.Models.Products;

public class Product : BaseModel
{
    public Product()
    { 
        DateAdded = DateTime.Now;
        Deleted = false;
    }

    [Key]
    public Int32 Id { get; set; }
    [Required]
    public String Name { get; set; }
    [Required]
    public Decimal Price { get; set; }
}
