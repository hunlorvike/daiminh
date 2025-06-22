using System.ComponentModel.DataAnnotations;

namespace shared.Models;

public class EmailSettings
{
    [Required(ErrorMessage = "Máy chủ SMTP không được để trống.")]
    public string SmtpServer { get; set; } = string.Empty;

    [Range(1, 65535, ErrorMessage = "Cổng SMTP phải từ 1 đến 65535.")]
    public int SmtpPort { get; set; } = 587;

    [Required(ErrorMessage = "Tên đăng nhập SMTP không được để trống.")]
    public string SmtpUsername { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mật khẩu SMTP không được để trống.")]
    [DataType(DataType.Password)]
    public string SmtpPassword { get; set; } = string.Empty;

    public bool EnableSsl { get; set; } = true;

    [Required(ErrorMessage = "Email người gửi không được để trống.")]
    [EmailAddress]
    public string SenderEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tên người gửi không được để trống.")]
    public string SenderName { get; set; } = string.Empty;
}
