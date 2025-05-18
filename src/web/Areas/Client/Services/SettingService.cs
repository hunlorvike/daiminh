using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace web.Areas.Client.Services;

public partial class SettingService : ISettingService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDistributedCache _cache;
    private readonly ILogger<SettingService> _logger;
    private string GetSettingCacheKey(string key, string category) => $"Setting_{category}_{key}";
    private const string AllSettingsCacheKey = "AllSettings";
    private static readonly Setting NullSettingMarker = new() { Id = -1 };
    private static readonly byte[] NullSettingMarkerBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(NullSettingMarker));

    private readonly DistributedCacheEntryOptions _settingCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
    };

    private readonly DistributedCacheEntryOptions _notFoundCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
    };

    private readonly DistributedCacheEntryOptions _allSettingsCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
    };

    public SettingService(ApplicationDbContext dbContext, IDistributedCache cache, ILogger<SettingService> logger)
    {
        _dbContext = dbContext;
        _cache = cache;
        _logger = logger;
    }

    public async Task<string?> GetValue(string key, string category, string? defaultValue = null)
    {
        var cacheKey = GetSettingCacheKey(key, category);
        byte[]? cachedBytes = null;
        Setting? setting = null;

        try
        {
            cachedBytes = await _cache.GetAsync(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Không thể lấy cài đặt '{category}:{key}' từ cache.");
        }

        if (cachedBytes != null)
        {
            setting = DeserializeSetting(cachedBytes);
            if (setting?.Id == NullSettingMarker.Id)
            {
                _logger.LogDebug($"Cài đặt '{category}:{key}' được tìm thấy trong cache dưới dạng 'không tìm thấy'.");
                return defaultValue;
            }
            _logger.LogDebug($"Cài đặt '{category}:{key}' được tìm thấy trong cache.");
        }

        if (setting == null)
        {
            _logger.LogDebug($"Cài đặt '{category}:{key}' không có trong cache. Đang lấy từ cơ sở dữ liệu...");
            try
            {
                setting = await _dbContext.Settings
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(s => s.Key == key && s.Category == category && s.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không thể lấy cài đặt '{category}:{key}' từ cơ sở dữ liệu.");
                return defaultValue;
            }

            if (setting != null)
            {
                byte[] bytesToCache = SerializeSetting(setting);
                if (bytesToCache.Length > 0)
                {
                    try
                    {
                        await _cache.SetAsync(cacheKey, bytesToCache, _settingCacheOptions);
                        _logger.LogDebug($"Cài đặt '{category}:{key}' đã được lưu vào cache thành công.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Không thể lưu cài đặt '{category}:{key}' vào cache.");
                    }
                }
            }
            else
            {
                _logger.LogWarning($"Cài đặt '{category}:{key}' không tìm thấy hoặc không hoạt động trong cơ sở dữ liệu. Đang lưu 'không tìm thấy' vào cache.");
                try
                {
                    await _cache.SetAsync(cacheKey, NullSettingMarkerBytes, _notFoundCacheOptions);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Không thể lưu 'không tìm thấy' vào cache cho cài đặt '{category}:{key}'.");
                }

                return defaultValue;
            }
        }

        if (!string.IsNullOrEmpty(setting.Value))
        {
            return setting.Value;
        }

        if (!string.IsNullOrEmpty(setting.DefaultValue))
        {
            return setting.DefaultValue;
        }

        return defaultValue;
    }

    public async Task<T?> GetValue<T>(string key, string category, T? defaultValue = default)
    {
        var stringValue = await GetValue(key, category);

        if (string.IsNullOrEmpty(stringValue))
        {
            return defaultValue;
        }

        try
        {
            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(stringValue, out bool boolValue))
                {
                    return (T)(object)boolValue;
                }
            }
            else if (typeof(T).IsEnum)
            {
                try
                {
                    return (T)Enum.Parse(typeof(T), stringValue, true);
                }
                catch (ArgumentException)
                {
                    _logger.LogWarning($"Setting value '{stringValue}' for Key '{key}', Category '{category}' cannot be parsed as enum type '{typeof(T).Name}'.");
                }
            }

            return (T)Convert.ChangeType(stringValue, typeof(T));
        }
        catch (InvalidCastException)
        {
            _logger.LogError($"Setting value '{stringValue}' for Key '{key}', Category '{category}' cannot be converted to type '{typeof(T).Name}'. InvalidCastException.");
        }
        catch (FormatException)
        {
            _logger.LogError($"Setting value '{stringValue}' for Key '{key}', Category '{category}' cannot be converted to type '{typeof(T).Name}'. FormatException.");
        }
        catch (OverflowException)
        {
            _logger.LogError($"Setting value '{stringValue}' for Key '{key}', Category '{category}' cannot be converted to type '{typeof(T).Name}'. OverflowException.");
        }
        catch (ArgumentNullException)
        {
            _logger.LogError($"Setting value '{stringValue}' for Key '{key}', Category '{category}' cannot be converted to type '{typeof(T).Name}'. ArgumentNullException.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while converting setting value '{stringValue}' to type '{typeof(T).Name}' for Key '{key}', Category '{category}'.");
        }

        return defaultValue;
    }

    public async Task<IEnumerable<Setting>> GetAllSettings()
    {
        byte[]? cachedBytes = null;
        IEnumerable<Setting>? settings = null;

        try
        {
            cachedBytes = await _cache.GetAsync(AllSettingsCacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Không thể lấy tất cả cài đặt từ cache.");
        }

        if (cachedBytes != null)
        {
            settings = DeserializeSettingsList(cachedBytes);
            if (settings != null)
            {
                _logger.LogDebug("Danh sách tất cả cài đặt được tìm thấy trong cache.");
                return settings;
            }
            _logger.LogWarning("Không thể giải mã danh sách cài đặt từ cache. Đang lấy từ cơ sở dữ liệu...");
        }

        _logger.LogDebug("Danh sách tất cả cài đặt không có trong cache. Đang lấy từ cơ sở dữ liệu...");
        try
        {
            settings = await _dbContext.Settings
                                    .AsNoTracking()
                                    .Where(s => s.IsActive)
                                    .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Không thể lấy tất cả cài đặt từ cơ sở dữ liệu.");
            return Enumerable.Empty<Setting>();
        }

        byte[] bytesToCache = SerializeSettingsList(settings);
        if (bytesToCache.Length > 0)
        {
            try
            {
                await _cache.SetAsync(AllSettingsCacheKey, bytesToCache, _allSettingsCacheOptions);
                _logger.LogDebug("Danh sách tất cả cài đặt đã được lưu vào cache thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Không thể lưu danh sách tất cả cài đặt vào cache.");
            }
        }

        return settings ?? Enumerable.Empty<Setting>();
    }

    public async Task ClearCache(string key, string category)
    {
        var cacheKey = GetSettingCacheKey(key, category);
        try
        {
            await _cache.RemoveAsync(cacheKey);
            _logger.LogInformation($"Đã xóa cache cho cài đặt '{category}:{key}'.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Không thể xóa cache cho cài đặt '{category}:{key}'.");
        }

        await ClearAllCache();
    }

    public async Task ClearAllCache()
    {
        try
        {
            await _cache.RemoveAsync(AllSettingsCacheKey);
            _logger.LogInformation("Đã xóa toàn bộ cache (xóa AllSettingsCacheKey).");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Không thể xóa toàn bộ cache.");
        }
    }
}

public partial class SettingService
{

    private byte[] SerializeSetting(Setting setting)
    {
        try
        {
            var json = JsonSerializer.Serialize(setting);
            return Encoding.UTF8.GetBytes(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to serialize Setting object.");
            return Array.Empty<byte>();
        }
    }

    private Setting? DeserializeSetting(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0) return null;
        try
        {
            var json = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<Setting>(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize Setting object.");
            return null;
        }
    }

    private byte[] SerializeSettingsList(IEnumerable<Setting> settings)
    {
        try
        {
            var json = JsonSerializer.Serialize(settings);
            return Encoding.UTF8.GetBytes(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to serialize Setting list.");
            return Array.Empty<byte>();
        }
    }

    private IEnumerable<Setting>? DeserializeSettingsList(byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0) return null;
        try
        {
            var json = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<IEnumerable<Setting>>(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize Setting list.");
            return null;
        }
    }
}