using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TMP_API.Entities;
using TMP_API.Helpers;
using TMP_API.Models.Orders;
using TMP_API.Repository.IRepository;

namespace TMP_API.Services.IServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _order;
        private readonly IOrderItemRepository _orderItem;

        public OrderService(IOrderRepository order, IOrderItemRepository orderItem)
        {
            _order = order;
            _orderItem = orderItem;
        }

        public async Task<ApiResponse> PostOrder(CreateOrderDto model)
        {
            try
            {
                Order value = new();
                value.InjectFrom(model);

                value.TotalPrice = model.OrderItems.Sum(s => s.Amount);
                await _order.InsertAsync(value);

                foreach (var order in model.OrderItems)
                {
                    var orderItem = _orderItem.Query().Where(oi => oi.Id == order.Id).FirstOrDefaultAsync().Result;
                    if (orderItem != null)
                    {
                        orderItem.OrderId = value.Id;
                        _orderItem.UpdateAsync(orderItem);
                    }
                }

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

        public async Task<ApiPaginatedResponse<List<OrderDto>>> GetAll(string search, int page, int limit, int skip)
        {
            try
            {
                var query = _order.Query();
                if (!string.IsNullOrEmpty(search))
                {
                    var searchLower = search.ToLower();
                    query = query.Where(q => q.User.FirstName.ToLower().Equals(searchLower) || q.User.LastName.ToLower().Equals(searchLower)
                    || q.User.Address.ToLower().Equals(searchLower) || q.User.PhoneNumber.ToLower().Equals(searchLower))
                        .OrderByDescending(d => d.DateAdded).Include(o => o.OrderItems).AsQueryable();
                }
                else
                {
                    query = query.OrderByDescending(d => d.DateAdded).Include(o => o.OrderItems).AsQueryable();
                }

                var data = await query.Skip(skip).Take(limit).Select(p => new OrderDto
                {
                    Id = p.Id,
                    OrderItems = p.OrderItems,
                    Customer = p.User,
                    OrderPrice = p.TotalPrice,
                    OrderDate = p.DateAdded
                }).ToListAsync();

                if (data.Count > 0)
                {
                    return new ApiPaginatedResponse<List<OrderDto>>
                    {
                        Success = true,
                        TotalCount = query.Count(),
                        Current_Page = page,
                        Limit = limit,
                        Data = data
                    };
                }
                return new ApiPaginatedResponse<List<OrderDto>>
                {
                    Message = ResponseMessages.NoRecordFound
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ResponseMessages.InternalServerError);
            }
        }

        public async Task<ApiResponse<OrderDto>> GetOrder(int OrderId)
        {
            try
            {
                var data = await _order.Query().Where(x => x.Id == OrderId)
                    .Include(o => o.OrderItems).Select(p => new OrderDto
                {
                    Id = p.Id,
                    OrderItems = p.OrderItems,
                    Customer = p.User,
                    OrderPrice = p.TotalPrice,
                    OrderDate = p.DateAdded
                }).FirstOrDefaultAsync();

                if (data != null)
                {
                    return new ApiResponse<OrderDto>
                    {
                        Success = true,
                        Data = data
                    };
                }

                return new ApiResponse<OrderDto>
                {
                    Message = ResponseMessages.NoRecordFound
                };

            }
            catch (Exception e)
            {
                throw new Exception(ResponseMessages.InternalServerError);
            }
        }

        public async Task<ApiResponse<List<OrderDto>>> GetUserOrder(Guid userId)
        {
            try
            {
                var data = await _order.Query().Where(x => x.UserId == userId)
                    .Include(o => o.OrderItems).Select(p => new OrderDto
                {
                    Id = p.Id,
                    OrderItems = p.OrderItems,
                    Customer = p.User,
                    OrderPrice = p.TotalPrice,
                    OrderDate = p.DateAdded
                }).ToListAsync();

                if (data.Count > 0)
                {
                    return new ApiResponse<List<OrderDto>>
                    {
                        Success = true,
                        Data = data
                    };
                }

                return new ApiResponse<List<OrderDto>>
                {
                    Message = ResponseMessages.NoRecordFound
                };

            }
            catch (Exception e)
            {
                throw new Exception(ResponseMessages.InternalServerError);
            }
        }

        public async Task<ApiResponse> UpdateOrder(CreateOrderDto model, int id)
        {
            try
            {
                var value = await _order.GetAsync(id);
                if (value == null) throw new Exception(ResponseMessages.NoRecordFound);

                value.InjectFrom(model);

                value.TotalPrice = model.OrderItems.Sum(s => s.Amount);
                await _order.UpdateAsync(value);

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

        public async Task<ApiResponse> DeleteOrder(int id)
        {
            try
            {
                var value = await _order.GetAsync(id);

                if (value == null) throw new Exception(ResponseMessages.NoRecordFound);

                await _order.UpdateAsync(value);

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
    }
}
