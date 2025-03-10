using domain.Entities;
using shared.Models;

namespace application.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(Category model);
    Task<BaseResponse> UpdateAsync(int id, Category model);
    Task<BaseResponse> DeleteAsync(int id);
}