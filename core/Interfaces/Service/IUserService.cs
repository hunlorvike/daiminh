using Core.Common.Models;
using core.Entities;

namespace core.Interfaces.Service;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(User user);
    Task<BaseResponse> UpdateAsync(int id, User user);
}