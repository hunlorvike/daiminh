using System.ComponentModel.DataAnnotations;

namespace shared.Models;

public class GeneralSettings
{
    [Required(ErrorMessage = "Tên trang web không được để trống.")]
    [StringLength(100)]
    public string SiteName { get; set; } = "Sơn Đại Minh";

    [StringLength(255)]
    public string SiteSlogan { get; set; } = "Giải pháp toàn diện cho mọi công trình";

    [StringLength(500)]
    public string SiteDescription { get; set; } = "Cung cấp sơn và vật liệu chống thấm chính hãng, dịch vụ thi công chuyên nghiệp.";

    [Required(ErrorMessage = "Email liên hệ chính không được để trống.")]
    [EmailAddress]
    public string ContactEmail { get; set; } = "sondaiminh@gmail.com";

    [Required(ErrorMessage = "Số điện thoại liên hệ chính không được để trống.")]
    [Phone]
    public string ContactPhoneNumber { get; set; } = "0979758340";

    [StringLength(255)]
    public string Address { get; set; } = "Tiên Phương, Chương Mỹ, Hà Nội";

    public string? ClientLogoUrl { get; set; } = "/img/icon.jpg";

    public string? AdminLogoUrl { get; set; } = "/img/icon.jpg";

    public string? FaviconUrl { get; set; } = "/img/icon.jpg";
    public string DefaultLanguage { get; set; } = "vi-VN";
    public string DateTimeFormat { get; set; } = "dd/MM/yyyy HH:mm";
    public string DateFormat { get; set; } = "dd/MM/yyyy";
}