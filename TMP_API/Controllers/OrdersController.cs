using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TMP_API.Helpers;
using TMP_API.Models.Orders;
using TMP_API.Services.IServices;
using static TMP_API.Services.UserService;

namespace TMP_API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedResponse<List<OrderDto>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetAll([FromQuery] string? search = null, int page = 1, int limit = 10)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());
        try
        {
            int skip = 0;
            if (page != 1) { skip = limit * (page - 1); }

            var result = await _orderService.GetAll(search, page, limit, skip);

            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        };
    }

    // GET api/Order
    [HttpGet("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<OrderDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());
        try
        {
            var result = await _orderService.GetOrder(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        };
    }
    [HttpGet("[action]/{user}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<OrderDto>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetByUser([FromRoute] string user)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());
        try
        {
            var result = await _orderService.GetUserOrder(user);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        };
    }
    // POST api/Order
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    [HttpPost("[action]")]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto value)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());

        try
        {
            var user = User.Identity.Name;
            var result = await _orderService.PostOrder(value, user);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        }
    }

    [HttpPut("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Put([FromBody] CreateOrderDto value, [FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _orderService.UpdateOrder(value, id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        }
    }


    // DELETE api/Order
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var result = await _orderService.DeleteOrder(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        }
    }
}
