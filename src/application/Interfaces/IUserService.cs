using shared.Models;
using domain.Entities;

namespace application.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(User user);
    Task<BaseResponse> UpdateAsync(int id, User user);
}