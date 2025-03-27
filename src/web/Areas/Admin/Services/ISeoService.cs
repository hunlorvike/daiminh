using web.Areas.Admin.ViewModels.Seo;

namespace web.Areas.Admin.Services;

public interface ISeoService
{
    // SeoSettings
    Task<List<SeoSettingsListItemViewModel>> GetAllSettingsAsync();
    Task<SeoSettingsViewModel?> GetSettingByIdAsync(int id);
    Task<SeoSettingsViewModel?> GetSettingByKeyAsync(string key);
    Task<SeoGeneralSettingsViewModel> GetGeneralSettingsAsync();
    Task<int> CreateSettingAsync(SeoSettingsViewModel model);
    Task UpdateSettingAsync(SeoSettingsViewModel model);
    Task UpdateGeneralSettingsAsync(SeoGeneralSettingsViewModel model);
    Task DeleteSettingAsync(int id);

    // SeoAnalytics
    Task<List<SeoAnalyticsListItemViewModel>> GetAnalyticsAsync(
        string? entityType = null,
        int? entityId = null,
        DateTime? startDate = null,
        DateTime? endDate = null);
    Task<SeoAnalyticsSummaryViewModel> GetAnalyticsSummaryAsync(
        string? entityType = null,
        DateTime? startDate = null,
        DateTime? endDate = null);
    Task<SeoAnalyticsViewModel?> GetAnalyticsByIdAsync(int id);
    Task<int> CreateAnalyticsAsync(SeoAnalyticsViewModel model);
    Task UpdateAnalyticsAsync(SeoAnalyticsViewModel model);
    Task DeleteAnalyticsAsync(int id);

    // Import/Export
    Task ImportFromGoogleSearchConsoleAsync(IFormFile jsonFile, DateTime startDate, DateTime endDate, bool overwriteExisting);
    Task<byte[]> ExportAnalyticsToExcelAsync(
        string? entityType = null,
        int? entityId = null,
        DateTime? startDate = null,
        DateTime? endDate = null);

    // Helpers
    Task<string> GetSeoSettingValueAsync(string key, string defaultValue = "");
    Task<Dictionary<string, string>> GetAllSeoSettingValuesAsync();
    Task<string> GenerateMetaTagsAsync(
        string? title = null,
        string? description = null,
        string? keywords = null,
        string? imageUrl = null,
        string? canonicalUrl = null);
}
