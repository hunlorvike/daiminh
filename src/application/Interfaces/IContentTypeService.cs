using domain.Entities;
using shared.Models;

namespace application.Interfaces;

public interface IContentTypeService
{
    Task<List<ContentType>> GetAllAsync();
    Task<ContentType?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(ContentType model);
    Task<BaseResponse> UpdateAsync(int id, ContentType model);
    Task<BaseResponse> DeleteAsync(int id);
}