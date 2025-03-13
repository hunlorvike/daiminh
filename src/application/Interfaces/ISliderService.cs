using domain.Entities;
using shared.Models;

namespace application.Interfaces;

public interface ISliderService
{
    Task<List<Slider>> GetAllAsync();
    Task<Slider?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(Slider model);
    Task<BaseResponse> UpdateAsync(int id, Slider model);
    Task<BaseResponse> DeleteAsync(int id);
}