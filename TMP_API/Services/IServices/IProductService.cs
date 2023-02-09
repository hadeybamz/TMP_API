using TMP_API.Helpers;
using TMP_API.Models.Products.Dtos;

namespace TMP_API.Services.IServices
{
    public interface IProductService
    {
        Task<ApiResponse> PostAccount(CreateProductDto model);
        Task<ApiPaginatedResponse<List<ProductDto>>> GetAll(string search, int page, int limit, int skip);
        Task<ApiResponse<ProductDto>> GetAccount(int Id);
        Task<ApiResponse> UpdateAccount(CreateProductDto model, int id);
        Task<ApiResponse> DeleteAccount(int id);
    }
}