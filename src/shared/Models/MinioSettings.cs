using System.ComponentModel.DataAnnotations;

namespace shared.Models;

public class MinioSettings
{
    [Required(ErrorMessage = "Endpoint không được để trống.")]
    public string Endpoint { get; set; } = "http://localhost:9000";

    [Required(ErrorMessage = "AccessKey không được để trống.")]
    public string AccessKey { get; set; } = "minioadmin";

    [Required(ErrorMessage = "SecretKey không được để trống.")]
    public string SecretKey { get; set; } = "minioadmin";

    [Required(ErrorMessage = "Tên Bucket không được để trống.")]
    public string BucketName { get; set; } = "daiminh";

    public string? PublicBaseUrl { get; set; } // VD: "http://cdn.example.com/daiminh" hoặc giống Endpoint + BucketName

    public bool UseSSL { get; set; } = false;

    [Range(60, 86400, ErrorMessage = "Thời gian hết hạn URL phải từ 60 đến 86400 giây.")] // 1 phút đến 1 ngày
    public int PresignedUrlExpirySeconds { get; set; } = 3600; // Mặc định 1 giờ
}

