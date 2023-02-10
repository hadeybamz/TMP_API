using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using TMP_API.Entities;

namespace TMP_API.Models.Users;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }

}

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole(string name)
    {
        Name = name;
    }
}
