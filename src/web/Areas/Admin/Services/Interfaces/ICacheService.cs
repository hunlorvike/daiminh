using Microsoft.Extensions.Caching.Distributed;

namespace web.Areas.Admin.Services.Interfaces;

public interface ICacheService
{
    /// <summary>
    /// Lấy một mục từ cache.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của mục.</typeparam>
    /// <param name="key">Khóa của mục.</param>
    /// <param name="cancellationToken">Token để hủy bỏ hoạt động.</param>
    /// <returns>Giá trị từ cache hoặc default(T) nếu không tìm thấy.</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy hoặc tạo một mục trong cache.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của mục.</typeparam>
    /// <param name="key">Khóa của mục.</param>
    /// <param name="factory">Hàm factory để tạo mục nếu không có trong cache.</param>
    /// <param name="optionsFactory">Hàm factory để tạo DistributedCacheEntryOptions (tùy chọn).</param>
    /// <param name="cancellationToken">Token để hủy bỏ hoạt động.</param>
    /// <returns>Giá trị từ cache hoặc giá trị mới được tạo.</returns>
    Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<Task<T>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Thiết lập một mục vào cache.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của mục.</typeparam>
    /// <param name="key">Khóa của mục.</param>
    /// <param name="value">Giá trị để cache.</param>
    /// <param name="options">Tùy chọn cache (thời gian hết hạn, v.v.).</param>
    /// <param name="cancellationToken">Token để hủy bỏ hoạt động.</param>
    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Xóa một mục khỏi cache.
    /// </summary>
    /// <param name="key">Khóa của mục.</param>
    /// <param name="cancellationToken">Token để hủy bỏ hoạt động.</param>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Làm mới thời gian sống của một mục trong cache.
    /// </summary>
    /// <param name="key">Khóa của mục.</param>
    /// <param name="cancellationToken">Token để hủy bỏ hoạt động.</param>
    Task RefreshAsync(string key, CancellationToken cancellationToken = default);
}