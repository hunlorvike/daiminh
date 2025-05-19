using shared.Models;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Services.Interfaces;

public interface ISettingService
{
    Task<SettingsIndexViewModel> GetSettingsIndexViewModelAsync(string? searchTerm);

    Task<OperationResult> UpdateSettingsAsync(IEnumerable<SettingViewModel> settings);
}
