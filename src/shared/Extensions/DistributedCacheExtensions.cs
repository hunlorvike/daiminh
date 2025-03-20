using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace shared.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task SetObjectAsync<T>(this IDistributedCache cache,
        string key, T value, DistributedCacheEntryOptions? options = null)
    {
        string jsonData = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, jsonData, options ?? new DistributedCacheEntryOptions());
    }

    public static async Task<T?> GetObjectAsync<T>(this IDistributedCache cache, string key)
    {
        string? jsonData = await cache.GetStringAsync(key);

        if (string.IsNullOrEmpty(jsonData))
            return default;

        return JsonSerializer.Deserialize<T>(jsonData);
    }
}
