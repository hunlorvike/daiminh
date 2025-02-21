using Core.Common.Models;
using core.Entities;

namespace core.Interfaces.Service;

public interface IContentTypeService
{
    Task<List<ContentType>> GetAllAsync();
    Task<ContentType?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(ContentType model);
    Task<BaseResponse> UpdateAsync(int id, ContentType model);
    Task<BaseResponse> DeleteAsync(int id);
}