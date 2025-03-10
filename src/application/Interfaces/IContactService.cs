using domain.Entities;
using shared.Models;

namespace application.Interfaces;

public interface IContactService
{
    Task<List<Contact>> GetAllAsync();
    Task<Contact?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(Contact model);
    Task<BaseResponse> UpdateAsync(int id, Contact model);
    Task<BaseResponse> DeleteAsync(int id);
}