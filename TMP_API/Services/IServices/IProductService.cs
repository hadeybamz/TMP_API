using TMP_API.Helpers;
using TMP_API.Models.Products;

namespace TMP_API.Services.IServices;

public interface IProductService
{
    Task<ApiResponse> PostProduct(CreateProductDto model);
    Task<ApiPaginatedResponse<List<ProductDto>>> GetAll(string search, int page, int limit, int skip);
    Task<ApiResponse<ProductDto>> GetProduct(int Id);
    Task<ApiResponse> UpdateProduct(CreateProductDto model, int id);
    Task<ApiResponse> DeleteProduct(int id);
}