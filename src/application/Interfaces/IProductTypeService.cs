using domain.Entities;
using shared.Models;

namespace application.Interfaces;

public interface IProductTypeService
{
    Task<List<ProductType>> GetAllAsync();
    Task<ProductType?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(ProductType model);
    Task<BaseResponse> UpdateAsync(int id, ProductType model);
    Task<BaseResponse> DeleteAsync(int id);
}