using shared.Models;
using domain.Entities;

namespace application.Interfaces;

public interface IProductFieldDefinitionService
{
    Task<List<ProductFieldDefinition>> GetAllAsync();
    Task<ProductFieldDefinition?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(ProductFieldDefinition model);
    Task<BaseResponse> UpdateAsync(int id, ProductFieldDefinition model);
    Task<BaseResponse> DeleteAsync(int id);
}