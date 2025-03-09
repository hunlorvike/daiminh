using shared.Models;
using domain.Entities;

namespace application.Interfaces;

public interface ISubscriberService
{
    Task<List<Subscriber>> GetAllAsync();
    Task<Subscriber?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(Subscriber model);
    Task<BaseResponse> UpdateAsync(int id, Subscriber model);
    Task<BaseResponse> DeleteAsync(int id);
}