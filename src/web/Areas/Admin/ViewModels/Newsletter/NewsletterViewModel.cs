using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Newsletter;

public class NewsletterViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Địa chỉ Email", Prompt = "Nhập địa chỉ email đăng ký")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ.")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Tên người đăng ký", Prompt = "Nhập tên người đăng ký (không bắt buộc)")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? Name { get; set; }

    [Display(Name = "Đang hoạt động")]
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
}