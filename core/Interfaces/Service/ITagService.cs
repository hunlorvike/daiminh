using Core.Common.Models;
using core.Entities;

namespace core.Interfaces.Service;

public interface ITagService
{
    Task<List<Tag>> GetAllAsync();
    Task<Tag?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(Tag model);
    Task<BaseResponse> UpdateAsync(int id, Tag model);
    Task<BaseResponse> DeleteAsync(int id);
}