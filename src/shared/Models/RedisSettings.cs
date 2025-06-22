using System.ComponentModel.DataAnnotations;

namespace shared.Models;
public class RedisSettings
{
    [Required(ErrorMessage = "Chuỗi kết nối Redis không được để trống.")]
    public string ConnectionString { get; set; } = "localhost:6379";
    public string InstanceName { get; set; } = "DaiMinh_";
    public int DefaultCacheDurationMinutes { get; set; } = 60;
}
