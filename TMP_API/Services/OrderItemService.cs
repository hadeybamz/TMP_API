using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TMP_API.Entities;
using TMP_API.Helpers;
using TMP_API.Models.OrderItems;
using TMP_API.Models.Orders;
using TMP_API.Repository.IRepository;
using TMP_API.Services.IServices;

namespace TMP_API.Services;

public class OrderItemService : IOrderItemService
{
    private IOrderItemRepository _orderItem;

    public OrderItemService(IOrderItemRepository orderItem)
    {
        _orderItem = orderItem;
    }

    public async Task<ApiResponse> PostOrderItem(CreateOrderItemDto model, string user)
    {
        try
        {
            OrderItem value = new();
            value.InjectFrom(model);
            value.UserId = Guid.Parse(user);

            value.Amount = model.Product.Price * model.Quantity;
            await _orderItem.InsertAsync(value);

            return new ApiResponse
            {
                Success = true,
                Message = ResponseMessages.Created
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ResponseMessages.InternalServerError);
        }
    }

    public async Task<ApiPaginatedResponse<List<OrderItemDto>>> GetAll(string search, int page, int limit, int skip)
    {
        try
        {
            var query = _orderItem.Query();
            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(q => q.Product.Name.ToLower().Equals(searchLower)).OrderBy(d => d.DateAdded).Include(o => o.Product).AsQueryable();
            }
            else
            {
                query = query.AsQueryable();
            }

            var data = await query.Skip(skip).Take(limit).Select(p => new OrderItemDto
            {
                Id = p.Id,
                Product = p.Product,
                Quantity = p.Quantity,
                Amount = p.Amount, 
                OrderId = p.OrderId,
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
        catch (Exception ex)
        {
            throw new Exception(ResponseMessages.InternalServerError);
        }
    }

    public async Task<ApiResponse<OrderItemDto>> GetOrderItem(int id)
    {
        try
        {
            var data = await _orderItem.Query().Where(x => x.Id == id)
                .Include(o=>o.Product).Select(p => new OrderItemDto
                {
                    Id = p.Id,
                    Product = p.Product,
                    Quantity = p.Quantity,
                    Amount = p.Amount,
                    OrderId = p.OrderId,
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
        catch (Exception e)
        {
            throw new Exception(ResponseMessages.InternalServerError);
        }
    }

    public async Task<ApiResponse> UpdateOrderItem(CreateOrderItemDto model, int id)
    {
        try
        {
            var value = await _orderItem.GetAsync(id);
            if (value == null) throw new Exception(ResponseMessages.NoRecordFound);

            value.InjectFrom(model);

            value.Amount = model.Product.Price * model.Quantity;
            await _orderItem.UpdateAsync(value);

            return (new ApiResponse
            {
                Success = true,
                Message = ResponseMessages.Updated
            });
        }
        catch (Exception ex)
        {
            throw new Exception(ResponseMessages.InternalServerError);
        }
    }

    public async Task<ApiResponse> DeleteOrderItem(int id)
    {
        try
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
        catch (Exception ex)
        {
            throw new Exception(ResponseMessages.InternalServerError);
        }
    }

    public async Task<ApiResponse<List<OrderItemDto>>> GetUserOrderItems(Guid userId)
    {
        try
        {
            var data = await _orderItem.Query().Where(x => x.UserId == userId)
                .Include(o => o.Product).Select(p => new OrderItemDto
                {
                    Id = p.Id,
                    Product = p.Product,
                    Quantity = p.Quantity,
                    Amount = p.Amount,
                    OrderId = p.OrderId,
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
        catch (Exception e)
        {
            throw new Exception(ResponseMessages.InternalServerError);
        }
    }
    
}
