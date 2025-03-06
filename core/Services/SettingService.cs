using core.Attributes;
using core.Entities;
using core.Interfaces.Infrastructure;
using core.Interfaces.Service;
using Core.Common.Models;
using Microsoft.EntityFrameworkCore;


namespace core.Services
{
    public partial class SettingService(IUnitOfWork unitOfWork) : ScopedService, ISettingService
    {
        public async Task<List<Setting>> GetAllAsync()
        {
            try
            {
                var settingRepository = unitOfWork.GetRepository<Setting, int>();

                return await settingRepository
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Setting?> GetByIdAsync(int id)
        {
            try
            {
                var settingRepository = unitOfWork.GetRepository<Setting, int>();

                return await settingRepository
                    .Where(ct => ct.Id == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse> AddAsync(Setting model)
        {
            try
            {
                var settingRepository = unitOfWork.GetRepository<Setting, int>();

                var errors = new Dictionary<string, string[]>();

                var existingSettingKey = await settingRepository
                    .FirstOrDefaultAsync(ct => ct.Key == model.Key);

                if (existingSettingKey != null)
                    errors.Add(nameof(model.Key), ["Key đã tồn tại"]);

                if (errors.Count != 0) return new ErrorResponse(errors);

                await settingRepository.AddAsync(model);
                await unitOfWork.SaveChangesAsync();

                return new SuccessResponse<Setting>(model, "Thêm thành công.");
            }
            catch (Exception ex)
            {
                return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
            }
        }

        public async Task<BaseResponse> UpdateAsync(int id, Setting model)
        {
            try
            {
                var settingRepository = unitOfWork.GetRepository<Setting, int>();

                var existingKey = await settingRepository
                    .FirstOrDefaultAsync(ct => ct.Key == model.Key && ct.Id != id);

                if (existingKey != null)
                    return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Key), ["Key đã tồn tại"] }
                });

                var existingValue = await settingRepository
                    .FirstOrDefaultAsync(ct => ct.Value == model.Value && ct.Id != id);

                if (existingValue != null)
                    return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Value), ["Value đã tồn tại"] }
                });

                var existingGroup = await settingRepository
                   .FirstOrDefaultAsync(ct => ct.Group == model.Group && ct.Id != id);

                if (existingGroup != null)
                    return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Group), ["Group đã tồn tại"] }
                });

                var existingDescription = await settingRepository
                   .FirstOrDefaultAsync(ct => ct.Description == model.Description && ct.Id != id);

                if (existingDescription != null)
                    return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Group), ["Description đã tồn tại"] }
                });

                var existingOrder = await settingRepository
                  .FirstOrDefaultAsync(ct => ct.Order == model.Order && ct.Id != id);

                if (existingOrder != null)
                    return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Order), ["Order đã tồn tại"] }
                });

                var existingSetting = await settingRepository
                    .FirstOrDefaultAsync(ct => ct.Id == id);

                if (existingSetting == null)
                    return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Key-Value không tồn tại"] }
                });

                existingSetting.Key = model.Key ?? existingSetting.Key;
                existingSetting.Value = model.Value ?? existingSetting.Value;
                existingSetting.Group = model.Group ?? existingSetting.Group;
                existingSetting.Description = model.Description ?? existingSetting.Description;
                existingSetting.Order = model.Order != existingSetting.Order ? model.Order : existingSetting.Order;

                await unitOfWork.SaveChangesAsync();

                return new SuccessResponse<Setting>(existingSetting, "Cập nhật thành công.");
            }
            catch (Exception ex)
            {
                return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
            }
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            try
            {
                var settingRepository = unitOfWork.GetRepository<Setting, int>();
                var setting = await settingRepository.FirstOrDefaultAsync(ct => ct.Id == id);

                if (setting == null)
                    return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Loại sản phẩm không tồn tại."] } });

                setting.DeletedAt = DateTime.UtcNow;

                await unitOfWork.SaveChangesAsync();

                return new SuccessResponse<Setting>(setting, "Đã xóa thành công.");
            }
            catch (Exception ex)
            {
                return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
            }
        }
    }
}
