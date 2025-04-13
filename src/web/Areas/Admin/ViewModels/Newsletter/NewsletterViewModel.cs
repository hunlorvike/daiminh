using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Newsletter;

public class NewsletterViewModel
{
    public int Id { get; set; }

    [Display(Name = "Địa chỉ Email", Prompt = "Nhập địa chỉ email hợp lệ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [EmailAddress(ErrorMessage = "{0} không hợp lệ")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Tên người đăng ký", Prompt = "Nhập tên (không bắt buộc)")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? Name { get; set; }

    [Display(Name = "Trạng thái hoạt động")]
    public bool IsActive { get; set; } = true;

    // Read-only fields (might display in Edit view for info)
    [Display(Name = "Địa chỉ IP")]
    public string? IpAddress { get; set; }

    [Display(Name = "Trình duyệt/Thiết bị")]
    public string? UserAgent { get; set; }

    [Display(Name = "Ngày xác nhận")]
    public DateTime? ConfirmedAt { get; set; }

    [Display(Name = "Ngày hủy đăng ký")]
    public DateTime? UnsubscribedAt { get; set; }

    [Display(Name = "Ngày đăng ký")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Cập nhật lần cuối")]
    public DateTime? UpdatedAt { get; set; }
}