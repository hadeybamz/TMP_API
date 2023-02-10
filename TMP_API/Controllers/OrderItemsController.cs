using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
using TMP_API.Helpers;
using TMP_API.Models.OrderItems;
using TMP_API.Services.IServices;
using static TMP_API.Services.UserService;

namespace TMP_API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrderItemsController : ControllerBase
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemsController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedResponse<List<OrderItemDto>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetAll([FromQuery] string? search = null, int page = 1, int limit = 10)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());
        try
        {
            int skip = 0;
            if (page != 1) { skip = limit * (page - 1); }

            var result = await _orderItemService.GetAll(search, page, limit, skip);

            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        };
    }

    [HttpGet("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<OrderItemDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());
        try
        {
            var result = await _orderItemService.GetOrderItem(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        };
    }

    [HttpGet("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<OrderItemDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetUserOrderItems(Guid userId)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());
        try
        {
            var result = await _orderItemService.GetUserOrderItems(userId);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        };
    }


    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    [HttpPost("[action]")]
    public async Task<IActionResult> Create([FromBody] CreateOrderItemDto value)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());

        try
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _orderItemService.PostOrderItem(value, user);
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
    public async Task<IActionResult> Put([FromBody] CreateOrderItemDto value, [FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _orderItemService.UpdateOrderItem(value, id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var result = await _orderItemService.DeleteOrderItem(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        }
    }
}
