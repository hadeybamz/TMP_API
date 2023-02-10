﻿using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TMP_API.Entities;
using TMP_API.Helpers;
using TMP_API.Models.Products;
using TMP_API.Repository.IRepository;
using TMP_API.Services.IServices;

namespace TMP_API.Services;

public class ProductService : IProductService
{
    private IProductRepository _product;

    public ProductService(IProductRepository product)
    {
        _product = product;
    }

    public async Task<ApiResponse> PostProduct(CreateProductDto model, string user)
    {
        try
        {
            var check = await _product.Query().AnyAsync(m => m.Name == model.Name);
            if (check) throw new Exception(ResponseMessages.Exist);

            Product value = new();
            value.InjectFrom(model);

            value.UserId = Guid.Parse(user);
            await _product.InsertAsync(value);

            return new ApiResponse
            {
                Success = true,
                Message = ResponseMessages.Created
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ResponseMessages.InternalServerError, ex);
        }
    }

    public async Task<ApiPaginatedResponse<List<ProductDto>>> GetAll(string search, int page, int limit, int skip)
    {
        try
        {
            var query = _product.Query();
            if (!string.IsNullOrEmpty(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(q => q.Name.Equals(search) || q.Price.Equals(search)).OrderBy(d => d.DateAdded).AsQueryable();
            }
            else
            {
                query = query.AsQueryable();
            }

            var data = await query.Skip(skip).Take(limit).Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DateAdded = p.DateAdded
            }).ToListAsync();

            if(data.Count > 0)
            {
                return new ApiPaginatedResponse<List<ProductDto>>
                {
                    Success = true,
                    TotalCount = query.Count(),
                    Current_Page = page,
                    Limit = limit,
                    Data = data
                };
            }
            return new ApiPaginatedResponse<List<ProductDto>>
            {
                Message = ResponseMessages.NoRecordFound
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ResponseMessages.InternalServerError);
        }
    }

    public async Task<ApiResponse<ProductDto>> GetProduct(int Id)
    {
        try
        {
            var data = await _product.Query().Where(x => x.Id == Id).Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DateAdded = p.DateAdded
            }).FirstOrDefaultAsync();
            
            if(data != null)
            {
                return new ApiResponse<ProductDto>
                {
                    Success = true,
                    Data = data
                };
            }

            return new ApiResponse<ProductDto>
            {
                Message = ResponseMessages.NoRecordFound
            };

        }
        catch (Exception e)
        {
            throw new Exception(ResponseMessages.InternalServerError, e);
        }
    }

    public async Task<ApiResponse> UpdateProduct(CreateProductDto model, int id)
    {
        try
        {
            var value = await _product.GetAsync(id);

            if (value == null) throw new Exception(ResponseMessages.NoRecordFound);

            value.InjectFrom(model);


            await _product.UpdateAsync(value);

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


    public async Task<ApiResponse> DeleteProduct(int id)
    {
        try
        {
            var value = await _product.GetAsync(id);

            if (value == null) throw new Exception(ResponseMessages.NoRecordFound);

            await _product.DeleteAsync(value.Id);

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
