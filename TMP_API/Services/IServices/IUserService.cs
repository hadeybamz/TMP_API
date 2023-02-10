using TMP_API.Helpers;
using TMP_API.Models.Users;

namespace TMP_API.Services.IServices;

public interface IUserService
{
    Task<ApiResponse<UserRegisterResultDTO>> Register(UserRegisterDTO model);
    Task<ApiResponse<UserLoginResultDTO>> Login(UserLoginDTO userLoginDTO);
}