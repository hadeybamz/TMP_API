using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TMP_API.Services.IServices
{
    public interface IJwtTokenService
    {
        JwtSecurityToken GetJwtToken(List<Claim> userClaims);
    }
}
