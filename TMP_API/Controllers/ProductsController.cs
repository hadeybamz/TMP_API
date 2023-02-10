using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
using TMP_API.Helpers;
using TMP_API.Models.Products;
using TMP_API.Services.IServices;
using static TMP_API.Controllers.UserController;
using static TMP_API.Services.UserService;

namespace TMP_API.Controllers;

[Authorize(Roles = UserRoles.Admin)]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET api/Product/GetAll
    [HttpGet("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiPaginatedResponse<List<ProductDto>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetAll([FromQuery] string? search = null, int page = 1, int limit = 10)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());
        try
        {
            int skip = 0;
            if (page != 1) { skip = limit * (page - 1); }

            var result = await _productService.GetAll(search, page, limit, skip);

            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        };
    }

    // GET api/Product
    [HttpGet("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());
        try
        {
            var result = await _productService.GetProduct(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        };
    }
    // POST api/Product
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    [HttpPost("[action]")]
    public async Task<IActionResult> Create([FromBody] CreateProductDto value)
    {
        if (!ModelState.IsValid) throw new Exception(ModelState.ToString());

        try
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _productService.PostProduct(value, user);
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
    public async Task<IActionResult> Put([FromBody] CreateProductDto value, [FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _productService.UpdateProduct(value, id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        }
    }


    // DELETE api/Product
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var result = await _productService.DeleteProduct(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message);
            return BadRequest(new ApiResponse { Success = false, Message = "Request Failed", Reason = e.Message });
        }
    }
}
