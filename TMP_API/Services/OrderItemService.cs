using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TMP_API.Entities;
using TMP_API.Helpers;
using TMP_API.Models.OrderItems;
using TMP_API.Models.Orders;
using TMP_API.Models.Products;
using TMP_API.Repository.IRepository;
using TMP_API.Services.IServices;

namespace TMP_API.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _orderItem;
    private readonly IProductRepository _product;
    private readonly IUserService _user;

    public OrderItemService(IOrderItemRepository orderItem, IProductRepository product, IUserService user)
    {
        _orderItem = orderItem;
        _product = product;
        _user = user;
    }

    public async Task<ApiResponse> PostOrderItem(CreateOrderItemDto model, string user)
    {
        //Validate Product
        bool productExist = _product.Query().AnyAsync(p => p.Id == model.ProductId).Result;
        if (!productExist) throw new Exception("Invalid Product Id");

        var check = await _orderItem.Query().Where(p => p.ProductId == model.ProductId & p.User.UserName == user && p.OrderId == null).FirstOrDefaultAsync();
        if (check != null) await _orderItem.DeleteAsync(check.Id);

        OrderItem value = new();
        value.InjectFrom(model);
        var userId = _user.GetUserIdByName(user).Result;

        value.UserId = userId;

        await _orderItem.InsertAsync(value);

        return new ApiResponse
        {
            Success = true,
            Message = ResponseMessages.Created
        };
    }

    public async Task<ApiPaginatedResponse<List<OrderItemDto>>> GetAll(string search, int page, int limit, int skip)
    {
        var query = _orderItem.Query();
        if (!string.IsNullOrEmpty(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(q => q.Product.Name.ToLower().Equals(searchLower)).OrderBy(d => d.DateAdded).Include(o => o.Product).Include(o => o.User).AsQueryable();
        }
        else
        {
            query = query.OrderBy(d => d.DateAdded).Include(o => o.Product).Include(o => o.User).AsQueryable();
        }

        var data = await query.Skip(skip).Take(limit).Select(p => new OrderItemDto
        {
            Id = p.Id,
            Product = new ProductDto
            {
                Id = p.Product.Id,
                Name = p.Product.Name,
                Price = p.Product.Price,
            },
            Quantity = p.Quantity,
            Amount = p.Product.Price * p.Quantity, 
            OrderId = p.OrderId,
            User = p.User.UserName,
            DateAdded = p.DateAdded
        }).ToListAsync();

        if (data.Count > 0)
        {
            return new ApiPaginatedResponse<List<OrderItemDto>>
            {
                Success = true,
                TotalCount = query.Count(),
                Current_Page = page,
                Limit = limit,
                Data = data
            };
        }
        return new ApiPaginatedResponse<List<OrderItemDto>>
        {
            Message = ResponseMessages.NoRecordFound
        };
    }

    public async Task<ApiResponse<OrderItemDto>> GetOrderItem(int id)
    {
        var data = await _orderItem.Query().Where(x => x.Id == id)
            .Include(o=>o.Product).Include(p => p.User).Select(p => new OrderItemDto
            {
                Id = p.Id,
                Product = new ProductDto
                {
                    Id = p.Product.Id,
                    Name = p.Product.Name,
                    Price = p.Product.Price,
                },
                Quantity = p.Quantity,
                Amount = p.Product.Price * p.Quantity,
                OrderId = p.OrderId,
                User = p.User.UserName,
                DateAdded = p.DateAdded
            }).FirstOrDefaultAsync();

        if (data != null)
        {
            return new ApiResponse<OrderItemDto>
            {
                Success = true,
                Data = data
            };
        }

        return new ApiResponse<OrderItemDto>
        {
            Message = ResponseMessages.NoRecordFound
        };
    }

    public async Task<ApiResponse> UpdateOrderItem(CreateOrderItemDto model, int id)
    {
        var value = await _orderItem.GetAsync(id);
        if (value == null) throw new Exception(ResponseMessages.NoRecordFound);

        value.InjectFrom(model);

        await _orderItem.UpdateAsync(value);

        return (new ApiResponse
        {
            Success = true,
            Message = ResponseMessages.Updated
        });
    }
    

    public async Task<ApiResponse> DeleteOrderItem(int id)
    {
        var value = await _orderItem.GetAsync(id);

        if (value == null) throw new Exception(ResponseMessages.NoRecordFound);

        await _orderItem.DeleteAsync(value.Id);

        return new ApiResponse
        {
            Success = true,
            Message = ResponseMessages.Deleted
        };
    }
    
    public async Task<ApiResponse<List<OrderItemDto>>> GetUserOrderItems(string userId)
    {
        var data = await _orderItem.Query().Where(x => x.User.UserName == userId)
            .Include(o => o.Product).Select(p => new OrderItemDto
            {
                Id = p.Id,
                Product = new ProductDto
                {
                    Id = p.Product.Id,
                    Name = p.Product.Name,
                    Price = p.Product.Price,
                },
                Quantity = p.Quantity,
                Amount = p.Product.Price * p.Quantity,
                OrderId = p.OrderId,
                User = p.User.UserName,
                DateAdded = p.DateAdded
            }).ToListAsync();

        if (data.Count > 0)
        {
            return new ApiResponse<List<OrderItemDto>>
            {
                Success = true,
                Data = data
            };
        }

        return new ApiResponse<List<OrderItemDto>>
        {
            Message = ResponseMessages.NoRecordFound
        };
    }
    
}
