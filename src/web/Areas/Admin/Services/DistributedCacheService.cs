using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using web.Areas.Admin.Services.Interfaces;

namespace web.Areas.Admin.Services;

public partial class DistributedCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<DistributedCacheService> _logger;
    private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        // Thêm các tùy chọn khác nếu cần
    };

    public DistributedCacheService(IDistributedCache distributedCache, ILogger<DistributedCacheService> logger)
    {
        _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(cachedValue))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(cachedValue, _serializerOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi lấy dữ liệu từ cache với khóa {CacheKey}", key);
            return default;
        }
    }

    public async Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<Task<T>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null,
        CancellationToken cancellationToken = default)
    {
        var cachedValue = await GetAsync<T>(key, cancellationToken);
        if (cachedValue != null && !cachedValue.Equals(default(T))) // Kiểm tra default(T) vì struct không thể null
        {
            _logger.LogDebug("Cache hit cho khóa {CacheKey}", key);
            return cachedValue;
        }

        _logger.LogDebug("Cache miss cho khóa {CacheKey}. Đang tạo giá trị mới.", key);
        var newValue = await factory();

        if (newValue != null)
        {
            var options = optionsFactory?.Invoke() ?? GetDefaultCacheEntryOptions();
            await SetAsync(key, newValue, options, cancellationToken);
        }
        return newValue;
    }

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (value == null)
        {
            // Cân nhắc xem có nên xóa key nếu value là null hay không,
            // hoặc log một cảnh báo. Hiện tại, không làm gì cả để tránh cache một giá trị null.
            _logger.LogWarning("Không thể cache giá trị null cho khóa {CacheKey}", key);
            return;
        }

        try
        {
            var serializedValue = JsonSerializer.Serialize(value, _serializerOptions);
            var cacheOptions = options ?? GetDefaultCacheEntryOptions();
            await _distributedCache.SetStringAsync(key, serializedValue, cacheOptions, cancellationToken);
            _logger.LogDebug("Đã cache giá trị cho khóa {CacheKey}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi thiết lập dữ liệu vào cache với khóa {CacheKey}", key);
            // Không throw để ứng dụng vẫn hoạt động nếu cache lỗi
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            _logger.LogDebug("Đã xóa cache cho khóa {CacheKey}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi xóa dữ liệu từ cache với khóa {CacheKey}", key);
        }
    }

    public async Task RefreshAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _distributedCache.RefreshAsync(key, cancellationToken);
            _logger.LogDebug("Đã làm mới cache cho khóa {CacheKey}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi khi làm mới dữ liệu cache với khóa {CacheKey}", key);
        }
    }
}

public partial class DistributedCacheService
{
    // Helper để tạo options mặc định nếu không được cung cấp
    private DistributedCacheEntryOptions GetDefaultCacheEntryOptions()
    {
        return new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
        };
    }
}
