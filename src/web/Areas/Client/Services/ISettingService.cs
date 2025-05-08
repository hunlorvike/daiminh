using domain.Entities;

namespace web.Areas.Client.Services;

public interface ISettingService
{
    /// <summary>
    /// Lấy giá trị setting dưới dạng string.
    /// </summary>
    /// <param name="key">Key của setting (ví dụ: SiteName).</param>
    /// <param name="category">Category của setting (ví dụ: General).</param>
    /// <param name="defaultValue">Giá trị mặc định sẽ trả về nếu không tìm thấy setting, setting không active, hoặc giá trị setting (Value và DefaultValue) đều rỗng/null.</param>
    /// <returns>Giá trị setting (Value hoặc DefaultValue của entity) hoặc giá trị defaultValue parameter, hoặc null nếu không tìm thấy và defaultValue parameter là null.</returns>
    Task<string?> GetValue(string key, string category, string? defaultValue = null);

    /// <summary>
    /// Lấy giá trị setting và chuyển đổi sang kiểu dữ liệu mong muốn.
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu mong muốn.</typeparam>
    /// <param name="key">Key của setting.</param>
    /// <param name="category">Category của setting.</param>
    /// <param name="defaultValue">Giá trị mặc định sẽ trả về nếu không tìm thấy setting, setting không active, giá trị setting không thể parse sang T, hoặc giá trị setting (Value và DefaultValue) đều rỗng/null.</param>
    /// <returns>Giá trị setting đã chuyển đổi hoặc giá trị defaultValue parameter, hoặc giá trị default của kiểu T nếu không tìm thấy và defaultValue parameter là null.</returns>
    Task<T?> GetValue<T>(string key, string category, T? defaultValue = default);

    /// <summary>
    /// Lấy tất cả các settings active.
    /// </summary>
    /// <returns>Danh sách các Setting entity.</returns>
    Task<IEnumerable<Setting>> GetAllSettings();

    /// <summary>
    /// Xóa cache cho một setting cụ thể.
    /// </summary>
    Task ClearCache(string key, string category);

    /// <summary>
    /// Xóa toàn bộ cache settings (thường chỉ xóa cache tổng).
    /// </summary>
    Task ClearAllCache();
}