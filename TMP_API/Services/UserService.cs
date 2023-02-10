using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System.IdentityModel.Tokens.Jwt;
using TMP_API.Entities;
using TMP_API.Helpers;
using TMP_API.Models.Users;
using TMP_API.Repository.IRepository;
using TMP_API.Services.IServices;

namespace TMP_API.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IClaimsService _claimsService;
    private readonly IJwtTokenService _jwtTokenService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IClaimsService claimsService,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _claimsService = claimsService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<ApiResponse<UserRegisterResultDTO>> Register(UserRegisterDTO model)
    {
        try
        {
            UserRegisterResultDTO data;
            IdentityResult result;

            ApplicationUser newUser = new();

            newUser.InjectFrom(model);

            result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
                data = new UserRegisterResultDTO
                {
                    Succeeded = result.Succeeded,
                    Errors = result.Errors.Select(e => e.Description)
                };

            await SeedRoles();
            result = await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            data =  new UserRegisterResultDTO { Succeeded = true };

            return new ApiResponse<UserRegisterResultDTO>
            {
                Success = result.Succeeded,
                Data = data,
                Message = result.Succeeded ? ResponseMessages.Created : ""
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ResponseMessages.InternalServerError, ex);
        }
    }

    private async Task SeedRoles()
    {
        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new ApplicationRole(UserRoles.User));

        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new ApplicationRole(UserRoles.User));
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public async Task<ApiResponse<UserLoginResultDTO>> Login(UserLoginDTO userLoginDTO)
    {
        try
        {
            UserLoginResultDTO data;
            var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                var userClaims = await _claimsService.GetUserClaimsAsync(user);

                var token = _jwtTokenService.GetJwtToken(userClaims);

                data = new UserLoginResultDTO
                {
                    Token = new TokenDTO
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    }
                };
            }
            throw new Exception("The email and password combination was invalid.");
        }
        catch (Exception ex)
        {
            throw new Exception(ResponseMessages.InternalServerError, ex);
        }
    }
}
