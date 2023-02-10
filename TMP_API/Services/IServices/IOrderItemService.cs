using TMP_API.Helpers;
using TMP_API.Models.OrderItems;

namespace TMP_API.Services.IServices;

public interface IOrderItemService
{
    Task<ApiResponse> PostOrderItem(CreateOrderItemDto model);
    Task<ApiPaginatedResponse<List<OrderItemDto>>> GetAll(string search, int page, int limit, int skip);
    Task<ApiResponse<OrderItemDto>> GetOrderItem(int Id);
    Task<ApiResponse> UpdateOrderItem(CreateOrderItemDto model, int id);
    Task<ApiResponse> DeleteOrderItem(int id);
    Task<ApiResponse<List<OrderItemDto>>> GetUserOrderItems(Guid userId);
}