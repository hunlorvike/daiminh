using System.Text;
using System.Text.Json;
using AutoMapper;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Seo;

namespace web.Areas.Admin.Services;

public class SeoService : ISeoService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<SeoService> _logger;

    public SeoService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<SeoService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    #region SeoSettings

    public async Task<List<SeoSettingsListItemViewModel>> GetAllSettingsAsync()
    {
        var settings = await _context.Set<SeoSettings>()
            .OrderBy(s => s.Key)
            .ToListAsync();

        return _mapper.Map<List<SeoSettingsListItemViewModel>>(settings);
    }

    public async Task<SeoSettingsViewModel?> GetSettingByIdAsync(int id)
    {
        var setting = await _context.Set<SeoSettings>()
            .FirstOrDefaultAsync(s => s.Id == id);

        return setting != null ? _mapper.Map<SeoSettingsViewModel>(setting) : null;
    }

    public async Task<SeoSettingsViewModel?> GetSettingByKeyAsync(string key)
    {
        var setting = await _context.Set<SeoSettings>()
            .FirstOrDefaultAsync(s => s.Key == key);

        return setting != null ? _mapper.Map<SeoSettingsViewModel>(setting) : null;
    }

    public async Task<SeoGeneralSettingsViewModel> GetGeneralSettingsAsync()
    {
        var model = new SeoGeneralSettingsViewModel();
        var allSettings = await _context.Set<SeoSettings>()
            .Where(s => s.IsActive)
            .ToDictionaryAsync(s => s.Key, s => s.Value ?? string.Empty);

        // Map settings to view model
        model.DefaultTitle = GetSettingValue(allSettings, SeoSettings.DefaultTitle);
        model.DefaultDescription = GetSettingValue(allSettings, SeoSettings.DefaultDescription);
        model.DefaultKeywords = GetSettingValue(allSettings, SeoSettings.DefaultKeywords);
        model.SiteName = GetSettingValue(allSettings, SeoSettings.SiteName);
        model.GoogleAnalyticsId = GetSettingValue(allSettings, SeoSettings.GoogleAnalyticsId);
        model.GoogleTagManagerId = GetSettingValue(allSettings, SeoSettings.GoogleTagManagerId);
        model.FacebookAppId = GetSettingValue(allSettings, SeoSettings.FacebookAppId);
        model.TwitterUsername = GetSettingValue(allSettings, SeoSettings.TwitterUsername);
        model.RobotsContent = GetSettingValue(allSettings, SeoSettings.RobotsContent, "index, follow");
        model.SitemapSettings = GetSettingValue(allSettings, SeoSettings.SitemapSettings);
        model.StructuredDataOrganization = GetSettingValue(allSettings, SeoSettings.StructuredDataOrganization);
        model.StructuredDataWebsite = GetSettingValue(allSettings, SeoSettings.StructuredDataWebsite);
        model.CustomHeadCode = GetSettingValue(allSettings, SeoSettings.CustomHeadCode);
        model.CustomFooterCode = GetSettingValue(allSettings, SeoSettings.CustomFooterCode);

        return model;
    }

    public async Task<int> CreateSettingAsync(SeoSettingsViewModel model)
    {
        var setting = _mapper.Map<SeoSettings>(model);
        _context.Add(setting);
        await _context.SaveChangesAsync();
        return setting.Id;
    }

    public async Task UpdateSettingAsync(SeoSettingsViewModel model)
    {
        var setting = await _context.Set<SeoSettings>()
            .FirstOrDefaultAsync(s => s.Id == model.Id);

        if (setting == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy cài đặt SEO với ID {model.Id}");
        }

        _mapper.Map(model, setting);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateGeneralSettingsAsync(SeoGeneralSettingsViewModel model)
    {
        // Cập nhật hoặc tạo mới các cài đặt
        await UpdateOrCreateSettingAsync(SeoSettings.DefaultTitle, model.DefaultTitle, "Tiêu đề mặc định cho trang web");
        await UpdateOrCreateSettingAsync(SeoSettings.DefaultDescription, model.DefaultDescription, "Mô tả mặc định cho trang web");
        await UpdateOrCreateSettingAsync(SeoSettings.DefaultKeywords, model.DefaultKeywords, "Từ khóa mặc định cho trang web");
        await UpdateOrCreateSettingAsync(SeoSettings.SiteName, model.SiteName, "Tên trang web");
        await UpdateOrCreateSettingAsync(SeoSettings.GoogleAnalyticsId, model.GoogleAnalyticsId, "ID Google Analytics");
        await UpdateOrCreateSettingAsync(SeoSettings.GoogleTagManagerId, model.GoogleTagManagerId, "ID Google Tag Manager");
        await UpdateOrCreateSettingAsync(SeoSettings.FacebookAppId, model.FacebookAppId, "ID ứng dụng Facebook");
        await UpdateOrCreateSettingAsync(SeoSettings.TwitterUsername, model.TwitterUsername, "Tên người dùng Twitter");
        await UpdateOrCreateSettingAsync(SeoSettings.RobotsContent, model.RobotsContent, "Nội dung thẻ meta robots");
        await UpdateOrCreateSettingAsync(SeoSettings.SitemapSettings, model.SitemapSettings, "Cài đặt Sitemap");
        await UpdateOrCreateSettingAsync(SeoSettings.StructuredDataOrganization, model.StructuredDataOrganization, "Dữ liệu có cấu trúc cho tổ chức");
        await UpdateOrCreateSettingAsync(SeoSettings.StructuredDataWebsite, model.StructuredDataWebsite, "Dữ liệu có cấu trúc cho website");
        await UpdateOrCreateSettingAsync(SeoSettings.CustomHeadCode, model.CustomHeadCode, "Mã tùy chỉnh trong thẻ head");
        await UpdateOrCreateSettingAsync(SeoSettings.CustomFooterCode, model.CustomFooterCode, "Mã tùy chỉnh cuối trang");
    }

    public async Task DeleteSettingAsync(int id)
    {
        var setting = await _context.Set<SeoSettings>()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (setting != null)
        {
            _context.Remove(setting);
            await _context.SaveChangesAsync();
        }
    }

    #endregion

    #region SeoAnalytics

    public async Task<List<SeoAnalyticsListItemViewModel>> GetAnalyticsAsync(
        string? entityType = null,
        int? entityId = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var query = _context.Set<SeoAnalytics>().AsQueryable();

        if (!string.IsNullOrEmpty(entityType))
        {
            query = query.Where(a => a.EntityType == entityType);
        }

        if (entityId.HasValue)
        {
            query = query.Where(a => a.EntityId == entityId.Value);
        }

        if (startDate.HasValue)
        {
            query = query.Where(a => a.Date >= startDate.Value.Date);
        }

        if (endDate.HasValue)
        {
            query = query.Where(a => a.Date <= endDate.Value.Date);
        }

        var analytics = await query
            .OrderByDescending(a => a.Date)
            .ThenBy(a => a.EntityType)
            .ThenBy(a => a.EntityId)
            .ToListAsync();

        return _mapper.Map<List<SeoAnalyticsListItemViewModel>>(analytics);
    }

    public async Task<SeoAnalyticsSummaryViewModel> GetAnalyticsSummaryAsync(
        string? entityType = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var query = _context.Set<SeoAnalytics>().AsQueryable();

        if (!string.IsNullOrEmpty(entityType))
        {
            query = query.Where(a => a.EntityType == entityType);
        }

        if (startDate.HasValue)
        {
            query = query.Where(a => a.Date >= startDate.Value.Date);
        }

        if (endDate.HasValue)
        {
            query = query.Where(a => a.Date <= endDate.Value.Date);
        }

        var analytics = await query.ToListAsync();

        var summary = new SeoAnalyticsSummaryViewModel
        {
            TotalImpressions = analytics.Sum(a => a.Impressions),
            TotalClicks = analytics.Sum(a => a.Clicks),
            AverageCTR = analytics.Any() ? analytics.Average(a => a.CTR) : 0,
            AveragePosition = analytics.Any() ? analytics.Average(a => a.AveragePosition) : 0
        };

        // Tổng hợp dữ liệu theo ngày
        var dailyData = analytics
            .GroupBy(a => a.Date)
            .Select(g => new DailyAnalyticsViewModel
            {
                Date = g.Key,
                Impressions = g.Sum(a => a.Impressions),
                Clicks = g.Sum(a => a.Clicks),
                CTR = g.Sum(a => a.Impressions) > 0 ? (double)g.Sum(a => a.Clicks) / g.Sum(a => a.Impressions) * 100 : 0,
                AveragePosition = g.Average(a => a.AveragePosition)
            })
            .OrderBy(d => d.Date)
            .ToList();

        summary.DailyData = dailyData;

        // Tổng hợp các trang hiệu suất cao nhất
        var topPages = analytics
            .GroupBy(a => new { a.EntityUrl, a.EntityTitle })
            .Where(g => g.Key.EntityUrl != null && g.Key.EntityTitle != null)
            .Select(g => new TopPageViewModel
            {
                Url = g.Key.EntityUrl!,
                Title = g.Key.EntityTitle!,
                Impressions = g.Sum(a => a.Impressions),
                Clicks = g.Sum(a => a.Clicks),
                CTR = g.Sum(a => a.Impressions) > 0 ? (double)g.Sum(a => a.Clicks) / g.Sum(a => a.Impressions) * 100 : 0,
                AveragePosition = g.Average(a => a.AveragePosition)
            })
            .OrderByDescending(p => p.Clicks)
            .Take(10)
            .ToList();

        summary.TopPages = topPages;

        // Tổng hợp từ khóa hàng đầu
        var allKeywords = new Dictionary<string, KeywordViewModel>();

        foreach (var item in analytics.Where(a => !string.IsNullOrEmpty(a.TopKeywords)))
        {
            try
            {
                var keywords = JsonSerializer.Deserialize<List<KeywordViewModel>>(item.TopKeywords ?? "[]");
                if (keywords != null)
                {
                    foreach (var kw in keywords)
                    {
                        if (!allKeywords.ContainsKey(kw.Keyword))
                        {
                            allKeywords[kw.Keyword] = new KeywordViewModel
                            {
                                Keyword = kw.Keyword,
                                Impressions = 0,
                                Clicks = 0,
                                CTR = 0,
                                AveragePosition = 0
                            };
                        }

                        allKeywords[kw.Keyword].Impressions += kw.Impressions;
                        allKeywords[kw.Keyword].Clicks += kw.Clicks;
                        allKeywords[kw.Keyword].AveragePosition += kw.AveragePosition;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi phân tích dữ liệu từ khóa: {TopKeywords}", item.TopKeywords);
            }
        }

        // Tính toán CTR và vị trí trung bình cho từ khóa
        foreach (var kw in allKeywords.Values)
        {
            kw.CTR = kw.Impressions > 0 ? (double)kw.Clicks / kw.Impressions * 100 : 0;
            kw.AveragePosition = kw.AveragePosition / analytics.Count(a => !string.IsNullOrEmpty(a.TopKeywords));
        }

        summary.TopKeywords = allKeywords.Values
            .OrderByDescending(k => k.Clicks)
            .Take(10)
            .ToList();

        return summary;
    }

    public async Task<SeoAnalyticsViewModel?> GetAnalyticsByIdAsync(int id)
    {
        var analytics = await _context.Set<SeoAnalytics>()
            .FirstOrDefaultAsync(a => a.Id == id);

        return analytics != null ? _mapper.Map<SeoAnalyticsViewModel>(analytics) : null;
    }

    public async Task<int> CreateAnalyticsAsync(SeoAnalyticsViewModel model)
    {
        var analytics = _mapper.Map<SeoAnalytics>(model);
        _context.Add(analytics);
        await _context.SaveChangesAsync();
        return analytics.Id;
    }

    public async Task UpdateAnalyticsAsync(SeoAnalyticsViewModel model)
    {
        var analytics = await _context.Set<SeoAnalytics>()
            .FirstOrDefaultAsync(a => a.Id == model.Id);

        if (analytics == null)
        {
            throw new KeyNotFoundException($"Không tìm thấy dữ liệu phân tích SEO với ID {model.Id}");
        }

        _mapper.Map(model, analytics);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAnalyticsAsync(int id)
    {
        var analytics = await _context.Set<SeoAnalytics>()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (analytics != null)
        {
            _context.Remove(analytics);
            await _context.SaveChangesAsync();
        }
    }

    #endregion

    #region Import/Export

    public async Task ImportFromGoogleSearchConsoleAsync(IFormFile jsonFile, DateTime startDate, DateTime endDate, bool overwriteExisting)
    {
        try
        {
            using var stream = jsonFile.OpenReadStream();
            using var reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();

            var searchConsoleData = JsonSerializer.Deserialize<GoogleSearchConsoleData>(jsonContent);
            if (searchConsoleData == null || searchConsoleData.Rows == null)
            {
                throw new InvalidOperationException("Dữ liệu JSON không hợp lệ hoặc không có dữ liệu");
            }

            // Xử lý dữ liệu và lưu vào cơ sở dữ liệu
            foreach (var row in searchConsoleData.Rows)
            {
                if (string.IsNullOrEmpty(row.Keys?.Page))
                    continue;

                var url = row.Keys.Page;
                var entityType = "Page"; // Mặc định
                var entityId = 0;
                var entityTitle = url;

                // Phân tích URL để xác định loại đối tượng và ID
                // Đây là logic mẫu, bạn cần điều chỉnh theo cấu trúc URL của ứng dụng
                if (url.Contains("/products/"))
                {
                    entityType = "Product";
                    var segments = url.Split('/');
                    var idSegment = segments.LastOrDefault();
                    if (idSegment != null && int.TryParse(idSegment, out var id))
                    {
                        entityId = id;
                    }
                }
                else if (url.Contains("/articles/"))
                {
                    entityType = "Article";
                    var segments = url.Split('/');
                    var idSegment = segments.LastOrDefault();
                    if (idSegment != null && int.TryParse(idSegment, out var id))
                    {
                        entityId = id;
                    }
                }

                // Tìm hoặc tạo mới bản ghi phân tích
                var analytics = await _context.Set<SeoAnalytics>()
                    .FirstOrDefaultAsync(a =>
                        a.EntityType == entityType &&
                        a.EntityUrl == url &&
                        a.Date == row.Date.Date);

                if (analytics == null)
                {
                    analytics = new SeoAnalytics
                    {
                        EntityType = entityType,
                        EntityId = entityId,
                        EntityTitle = entityTitle,
                        EntityUrl = url,
                        Date = row.Date.Date
                    };
                    _context.Add(analytics);
                }
                else if (!overwriteExisting)
                {
                    continue;
                }

                // Cập nhật dữ liệu
                analytics.Impressions = row.Impressions;
                analytics.Clicks = row.Clicks;
                analytics.CTR = row.CTR;
                analytics.AveragePosition = row.Position;

                // Lưu từ khóa hàng đầu
                if (row.Keywords != null && row.Keywords.Any())
                {
                    var keywords = row.Keywords.Select(k => new KeywordViewModel
                    {
                        Keyword = k.Keyword,
                        Impressions = k.Impressions,
                        Clicks = k.Clicks,
                        CTR = k.CTR,
                        AveragePosition = k.Position
                    }).ToList();

                    analytics.TopKeywords = JsonSerializer.Serialize(keywords);
                }
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi nhập dữ liệu từ Google Search Console");
            throw new InvalidOperationException("Lỗi khi nhập dữ liệu: " + ex.Message);
        }
    }

    public async Task<byte[]> ExportAnalyticsToExcelAsync(
        string? entityType = null,
        int? entityId = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        // Lấy dữ liệu phân tích
        var analytics = await GetAnalyticsAsync(entityType, entityId, startDate, endDate);

        // Tạo file Excel và trả về dữ liệu dạng byte[]
        // Đây là phần mẫu, bạn cần triển khai logic tạo file Excel thực tế
        using var stream = new MemoryStream();
        // TODO: Implement Excel export logic
        return stream.ToArray();
    }

    #endregion

    #region Helpers

    public async Task<string> GetSeoSettingValueAsync(string key, string defaultValue = "")
    {
        var setting = await _context.Set<SeoSettings>()
            .FirstOrDefaultAsync(s => s.Key == key && s.IsActive);

        return setting?.Value ?? defaultValue;
    }

    public async Task<Dictionary<string, string>> GetAllSeoSettingValuesAsync()
    {
        var settings = await _context.Set<SeoSettings>()
            .Where(s => s.IsActive)
            .ToDictionaryAsync(s => s.Key, s => s.Value ?? string.Empty);

        return settings;
    }

    public async Task<string> GenerateMetaTagsAsync(
        string? title = null,
        string? description = null,
        string? keywords = null,
        string? imageUrl = null,
        string? canonicalUrl = null)
    {
        var settings = await GetAllSeoSettingValuesAsync();

        var defaultTitle = GetSettingValue(settings, SeoSettings.DefaultTitle);
        var defaultDescription = GetSettingValue(settings, SeoSettings.DefaultDescription);
        var defaultKeywords = GetSettingValue(settings, SeoSettings.DefaultKeywords);
        var siteName = GetSettingValue(settings, SeoSettings.SiteName);
        var twitterUsername = GetSettingValue(settings, SeoSettings.TwitterUsername);
        var facebookAppId = GetSettingValue(settings, SeoSettings.FacebookAppId);

        var finalTitle = !string.IsNullOrEmpty(title) ? title : defaultTitle;
        var finalDescription = !string.IsNullOrEmpty(description) ? description : defaultDescription;
        var finalKeywords = !string.IsNullOrEmpty(keywords) ? keywords : defaultKeywords;

        var metaTags = new StringBuilder();

        // Thẻ title
        metaTags.AppendLine($"<title>{finalTitle}</title>");

        // Thẻ meta cơ bản
        metaTags.AppendLine($"<meta name=\"description\" content=\"{finalDescription}\" />");
        if (!string.IsNullOrEmpty(finalKeywords))
        {
            metaTags.AppendLine($"<meta name=\"keywords\" content=\"{finalKeywords}\" />");
        }

        // Open Graph tags
        metaTags.AppendLine($"<meta property=\"og:title\" content=\"{finalTitle}\" />");
        metaTags.AppendLine($"<meta property=\"og:description\" content=\"{finalDescription}\" />");
        metaTags.AppendLine("<meta property=\"og:type\" content=\"website\" />");

        if (!string.IsNullOrEmpty(canonicalUrl))
        {
            metaTags.AppendLine($"<meta property=\"og:url\" content=\"{canonicalUrl}\" />");
            metaTags.AppendLine($"<link rel=\"canonical\" href=\"{canonicalUrl}\" />");
        }

        if (!string.IsNullOrEmpty(imageUrl))
        {
            metaTags.AppendLine($"<meta property=\"og:image\" content=\"{imageUrl}\" />");
        }

        if (!string.IsNullOrEmpty(siteName))
        {
            metaTags.AppendLine($"<meta property=\"og:site_name\" content=\"{siteName}\" />");
        }

        // Twitter Card tags
        metaTags.AppendLine("<meta name=\"twitter:card\" content=\"summary_large_image\" />");
        metaTags.AppendLine($"<meta name=\"twitter:title\" content=\"{finalTitle}\" />");
        metaTags.AppendLine($"<meta name=\"twitter:description\" content=\"{finalDescription}\" />");

        if (!string.IsNullOrEmpty(imageUrl))
        {
            metaTags.AppendLine($"<meta name=\"twitter:image\" content=\"{imageUrl}\" />");
        }

        if (!string.IsNullOrEmpty(twitterUsername))
        {
            metaTags.AppendLine($"<meta name=\"twitter:site\" content=\"{twitterUsername}\" />");
            metaTags.AppendLine($"<meta name=\"twitter:creator\" content=\"{twitterUsername}\" />");
        }

        // Facebook App ID
        if (!string.IsNullOrEmpty(facebookAppId))
        {
            metaTags.AppendLine($"<meta property=\"fb:app_id\" content=\"{facebookAppId}\" />");
        }

        return metaTags.ToString();
    }

    private async Task UpdateOrCreateSettingAsync(string key, string value, string description)
    {
        var setting = await _context.Set<SeoSettings>()
            .FirstOrDefaultAsync(s => s.Key == key);

        if (setting == null)
        {
            setting = new SeoSettings
            {
                Key = key,
                Value = value,
                Description = description,
                IsActive = true
            };
            _context.Add(setting);
        }
        else
        {
            setting.Value = value;
            setting.IsActive = true;
        }

        await _context.SaveChangesAsync();
    }

    private string GetSettingValue(Dictionary<string, string> settings, string key, string defaultValue = "")
    {
        return settings.TryGetValue(key, out var value) ? value : defaultValue;
    }

    #endregion
}

// Lớp hỗ trợ cho việc phân tích dữ liệu từ Google Search Console
public class GoogleSearchConsoleData
{
    public List<SearchConsoleRow>? Rows { get; set; }
}

public class SearchConsoleRow
{
    public SearchConsoleKeys? Keys { get; set; }
    public DateTime Date { get; set; }
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public double CTR { get; set; }
    public double Position { get; set; }
    public List<SearchConsoleKeyword>? Keywords { get; set; }
}

public class SearchConsoleKeys
{
    public string? Page { get; set; }
}

public class SearchConsoleKeyword
{
    public string Keyword { get; set; } = string.Empty;
    public int Impressions { get; set; }
    public int Clicks { get; set; }
    public double CTR { get; set; }
    public double Position { get; set; }
}

