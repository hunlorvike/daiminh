using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Services;

public class SettingService : ISettingService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<SettingService> _logger;

    public SettingService(ApplicationDbContext context, IMapper mapper, ILogger<SettingService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SettingsIndexViewModel> GetSettingsIndexViewModelAsync(string? searchTerm)
    {
        IQueryable<Setting> query = _context.Set<Setting>()
                                    .Where(s => s.IsActive)
                                    .OrderBy(s => s.Category)
                                    .ThenBy(s => s.Key)
                                    .AsNoTracking();


        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(s => s.Key.ToLower().Contains(lowerSearchTerm) ||
                                (s.Description != null && s.Description.ToLower().Contains(lowerSearchTerm)));
        }

        var allSettings = await query.ToListAsync();

        var groupedSettings = allSettings
            .Select(s => _mapper.Map<SettingViewModel>(s))
            .GroupBy(svm => allSettings.First(s => s.Id == svm.Id).Category)
            .ToDictionary(g => g.Key, g => g.ToList());

        var viewModel = new SettingsIndexViewModel
        {
            SettingGroups = groupedSettings,
            SearchTerm = searchTerm
        };

        _logger.LogInformation("Settings data retrieved successfully for search term: '{SearchTerm}'", searchTerm);

        return viewModel;
    }

    public async Task<OperationResult> UpdateSettingsAsync(IEnumerable<SettingViewModel> settings)
    {
        if (settings == null || !settings.Any())
        {
            _logger.LogWarning("UpdateSettingsAsync called with no settings.");
            return OperationResult.SuccessResult("Không có cài đặt nào được gửi để cập nhật.");
        }

        var settingIds = settings.Select(s => s.Id).ToList();

        var settingsInDb = await _context.Set<Setting>()
                                        .Where(s => settingIds.Contains(s.Id))
                                        .ToListAsync();

        var dict = settingsInDb.ToDictionary(s => s.Id);

        bool changed = false;

        foreach (var settingVM in settings)
        {
            if (dict.TryGetValue(settingVM.Id, out var settingEntity))
            {
                if (settingEntity.Value != settingVM.Value)
                {
                    settingEntity.Value = settingVM.Value;
                    _logger.LogInformation("Setting value changed: Key='{Key}', OldValue='{OldValue}', NewValue='{NewValue}'", settingEntity.Key, _context.Entry(settingEntity).OriginalValues[nameof(Setting.Value)], settingEntity.Value);
                    changed = true;
                }
            }
            else
            {
                _logger.LogWarning("Setting with ID {Id} not found in DB during update.", settingVM.Id);
            }
        }

        if (!changed)
        {
            _logger.LogInformation("No settings values were changed during update attempt.");
            return OperationResult.SuccessResult("Không có thay đổi nào cần lưu.");
        }

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Settings updated successfully.");
            return OperationResult.SuccessResult("Cập nhật cài đặt thành công.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while updating settings.");
            return OperationResult.FailureResult("Đã xảy ra lỗi khi lưu cài đặt. Vui lòng thử lại.", errors: new List<string> { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating settings.");
            return OperationResult.FailureResult("Đã xảy ra lỗi hệ thống khi cập nhật.", errors: new List<string> { ex.Message });
        }
    }
}
