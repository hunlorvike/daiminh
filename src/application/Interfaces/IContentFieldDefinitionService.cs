using shared.Models;
using domain.Entities;

namespace application.Interfaces;

public interface IContentFieldDefinitionService
{
    Task<List<ContentFieldDefinition>> GetAllAsync();
    Task<ContentFieldDefinition?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(ContentFieldDefinition model);
    Task<BaseResponse> UpdateAsync(int id, ContentFieldDefinition model);
    Task<BaseResponse> DeleteAsync(int id);
}