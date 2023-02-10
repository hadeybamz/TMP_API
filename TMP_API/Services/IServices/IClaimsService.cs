using System.Security.Claims;
using TMP_API.Models.Users;

namespace TMP_API.Services.IServices
{
    public interface IClaimsService
    {
        Task<List<Claim>> GetUserClaimsAsync(ApplicationUser user);
    }
}
