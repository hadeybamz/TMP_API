using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using TMP_API.Helpers;
using TMP_API.Models.Users;
using TMP_API.Services.IServices;

namespace TMP_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<UserRegisterResultDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO model)
        {
            if (!ModelState.IsValid) throw new Exception(ModelState.ToString());

            try
            {
                var result = await _userService.Register(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);
                return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
            }
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<UserLoginResultDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
        {
            if (!ModelState.IsValid) throw new Exception(ModelState.ToString());

            try
            {
                var result = await _userService.Login(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                Log.Fatal(e.Message);
                return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
            }
        }
    }
}
