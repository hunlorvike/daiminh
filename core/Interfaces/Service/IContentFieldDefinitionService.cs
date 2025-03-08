using Core.Common.Models;
using core.Entities;

namespace core.Interfaces.Service;

public interface IContentFieldDefinitionService
{
    Task<List<ContentFieldDefinition>> GetAllAsync();
    Task<ContentFieldDefinition?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(ContentFieldDefinition model);
    Task<BaseResponse> UpdateAsync(int id, ContentFieldDefinition model);
    Task<BaseResponse> DeleteAsync(int id);
}