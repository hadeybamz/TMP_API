using TMP_API.Helpers;
using TMP_API.Models.Orders;

namespace TMP_API.Services.IServices
{
    public interface IOrderService
    {
        Task<ApiResponse> PostOrder(CreateOrderDto model);
        Task<ApiPaginatedResponse<List<OrderDto>>> GetAll(string search, int page, int limit, int skip);
        Task<ApiResponse<OrderDto>> GetOrder(int Id);
        Task<ApiResponse<List<OrderDto>>> GetUserOrder(Guid userId);
        Task<ApiResponse> UpdateOrder(CreateOrderDto model, int id);
        Task<ApiResponse> DeleteOrder(int id);
    }
}