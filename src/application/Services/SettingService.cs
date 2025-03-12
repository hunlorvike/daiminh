using application.Interfaces;
using domain.Entities;
using infrastructure; // Namespace của ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

public partial class SettingService : ISettingService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    public SettingService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    public async Task<List<Setting>> GetAllAsync()
    {
        try
        {
            // Direct query
            return await _context.Settings
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
            // Direct query
            return await _context.Settings
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
            // Validation and direct add
            var errors = new Dictionary<string, string[]>();

            var existingSettingKey = await _context.Settings
                .FirstOrDefaultAsync(ct => ct.Key == model.Key);

            if (existingSettingKey != null)
                errors.Add(nameof(model.Key), ["Key đã tồn tại"]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            await _context.Settings.AddAsync(model);
            await _context.SaveChangesAsync();

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
            // Check for duplicates, excluding the current record
            var existingKey = await _context.Settings
                .FirstOrDefaultAsync(ct => ct.Key == model.Key && ct.Id != id);

            if (existingKey != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Key), ["Key đã tồn tại"] }
                });

            var existingValue = await _context.Settings
              .FirstOrDefaultAsync(ct => ct.Value == model.Value && ct.Id != id);

            if (existingValue != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Value), ["Value đã tồn tại"] }
                });

            // Find the existing record
            var existingSetting = await _context.Settings
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (existingSetting == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Key-Value không tồn tại"] }
                });

            // Update fields
            existingSetting.Key = model.Key ?? existingSetting.Key;
            existingSetting.Value = model.Value ?? existingSetting.Value;
            existingSetting.Group = model.Group ?? existingSetting.Group;
            existingSetting.Description = model.Description ?? existingSetting.Description;
            existingSetting.Order = model.Order != existingSetting.Order ? model.Order : existingSetting.Order;


            await _context.SaveChangesAsync();

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
            // Find and soft-delete
            var setting = await _context.Settings.FirstOrDefaultAsync(ct => ct.Id == id);

            if (setting == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Loại sản phẩm không tồn tại."] } }); // Corrected message

            setting.DeletedAt = DateTime.UtcNow; // Soft delete

            await _context.SaveChangesAsync();

            return new SuccessResponse<Setting>(setting, "Đã xóa thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", [ex.Message] } });
        }
    }
}