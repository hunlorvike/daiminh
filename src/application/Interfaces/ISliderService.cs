using domain.Entities;
using shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Interfaces;

public interface ISliderService
{
    Task<List<Slider>> GetAllAsync();
    Task<Slider?> GetByIdAsync(int id);
    Task<BaseResponse> AddAsync(Slider model);
    Task<BaseResponse> UpdateAsync(int id, Slider model);
    Task<BaseResponse> DeleteAsync(int id);
}
