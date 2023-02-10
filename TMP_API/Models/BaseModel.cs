using TMP_API.Models.Users;

namespace TMP_API.Models;

public class BaseModel
{
    public DateTime DateAdded { get; set; } = DateTime.Now;

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; }
}
