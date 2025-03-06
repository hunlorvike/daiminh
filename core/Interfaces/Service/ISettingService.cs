using Core.Common.Models;
using core.Entities;


namespace core.Interfaces.Service;

public interface ISettingService
{
    Task<List<Setting>> GetAllAsync();
    Task<Setting?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(Setting model);
    Task<BaseResponse> UpdateAsync(int id, Setting model);
    Task<BaseResponse> DeleteAsync(int id);
}

