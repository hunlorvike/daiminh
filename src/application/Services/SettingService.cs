using application.Interfaces;
using domain.Entities;
using infrastructure; // Namespace của ApplicationDbContext
using Microsoft.EntityFrameworkCore;
using shared.Models;

namespace application.Services;

/// <summary>
/// Service for managing settings.
/// </summary>
public partial class SettingService : ISettingService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingService"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public SettingService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all settings.
    /// </summary>
    /// <returns>A list of settings.</returns>
    public async Task<List<Setting>> GetAllAsync()
    {
        try
        {
            // Retrieve all settings from the database.
            return await _context.Settings
                .AsNoTracking()
                .Where(x => x.DeletedAt == null)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "GetAllAsync SettingService");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách cài đặt.", ex);
        }
    }

    /// <summary>
    /// Retrieves a setting by its ID.
    /// </summary>
    /// <param name="id">The ID of the setting.</param>
    /// <returns>The setting, or null if not found.</returns>
    public async Task<Setting?> GetByIdAsync(int id)
    {
        try
        {
            // Retrieve a setting by ID from the database.
            return await _context.Settings
                .AsNoTracking()
                .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null); // Use FirstOrDefaultAsync
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"GetByIdAsync SettingService with id: {id}");
            throw new Exception($"Đã xảy ra lỗi khi lấy thông tin cài đặt có ID: {id}.", ex);
        }
    }

    /// <summary>
    /// Adds a new setting.
    /// </summary>
    /// <param name="model">The setting to add.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> AddAsync(Setting model)
    {
        try
        {
            // Check for duplicate keys.
            var errors = new Dictionary<string, string[]>();

            var existingSettingKey = await _context.Settings
                .FirstOrDefaultAsync(ct => ct.Key == model.Key && ct.DeletedAt == null);

            if (existingSettingKey != null)
                errors.Add(nameof(model.Key), ["Key cài đặt đã tồn tại. Vui lòng chọn một key khác."]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            // Add the new setting to the database.
            await _context.Settings.AddAsync(model);
            await _context.SaveChangesAsync();

            return new SuccessResponse<Setting>(model, "Thêm cài đặt mới thành công.");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "AddAsync SettingService");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi thêm cài đặt mới. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Updates an existing setting.
    /// </summary>
    /// <param name="id">The ID of the setting to update.</param>
    /// <param name="model">The updated setting data.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> UpdateAsync(int id, Setting model)
    {
        try
        {
            // Check for duplicate keys (excluding the current record).
            var existingKey = await _context.Settings
                .FirstOrDefaultAsync(ct => ct.Key == model.Key && ct.Id != id && ct.DeletedAt == null);

            if (existingKey != null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { nameof(model.Key), ["Key cài đặt đã tồn tại. Vui lòng chọn một key khác."] }
                });

            // Find the existing setting by ID.
            var existingSetting = await _context.Settings
                .FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null); // Check for DeletedAt

            if (existingSetting == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Cài đặt không tồn tại hoặc đã bị xóa."] }
                });

            // Update the setting properties.  Use null-coalescing operator for strings.
            existingSetting.Key = model.Key ?? existingSetting.Key;
            existingSetting.Value = model.Value ?? existingSetting.Value;
            existingSetting.Group = model.Group ?? existingSetting.Group;
            existingSetting.Description = model.Description ?? existingSetting.Description;
            existingSetting.Order = model.Order; // No null check (int)


            await _context.SaveChangesAsync();

            return new SuccessResponse<Setting>(existingSetting, "Cập nhật cài đặt thành công.");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"UpdateAsync SettingService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", ["Đã xảy ra lỗi khi cập nhật cài đặt. Vui lòng thử lại sau."] }
            });
        }
    }

    /// <summary>
    /// Deletes a setting (soft delete).
    /// </summary>
    /// <param name="id">The ID of the setting to delete.</param>
    /// <returns>A BaseResponse indicating success or failure.</returns>
    public async Task<BaseResponse> DeleteAsync(int id)
    {
        try
        {
            // Find the setting by ID (only if not already soft-deleted).
            var setting = await _context.Settings.FirstOrDefaultAsync(ct => ct.Id == id && ct.DeletedAt == null);

            if (setting == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                    { { "General", ["Cài đặt không tồn tại hoặc đã bị xóa."] } });

            // Perform a soft delete by setting the DeletedAt property.
            setting.DeletedAt = DateTime.UtcNow; // Soft delete

            await _context.SaveChangesAsync();

            return new SuccessResponse<Setting>(setting, "Xóa cài đặt thành công (đã ẩn).");
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, $"DeleteAsync SettingService with id: {id}");
            return new ErrorResponse(new Dictionary<string, string[]> { { "General", ["Đã xảy ra lỗi khi xóa cài đặt. Vui lòng thử lại sau."] } });
        }
    }
}