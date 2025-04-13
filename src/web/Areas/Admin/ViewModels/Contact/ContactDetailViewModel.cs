using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Contact;

public class ContactDetailViewModel
{
    public int Id { get; set; }

    [Display(Name = "Họ và Tên")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Số điện thoại")]
    public string? Phone { get; set; }

    [Display(Name = "Chủ đề")]
    public string Subject { get; set; } = string.Empty;

    [Display(Name = "Nội dung tin nhắn")]
    public string Message { get; set; } = string.Empty;

    [Display(Name = "Tên công ty")]
    public string? CompanyName { get; set; }

    [Display(Name = "Chi tiết dự án")]
    public string? ProjectDetails { get; set; }

    [Display(Name = "Trạng thái")]
    [Required(ErrorMessage = "Vui lòng chọn {0}")]
    public ContactStatus Status { get; set; } = ContactStatus.New;

    [Display(Name = "Ghi chú của quản trị viên", Prompt = "Thêm ghi chú nội bộ về liên hệ này...")]
    public string? AdminNotes { get; set; }

    [Display(Name = "Ngày nhận")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Địa chỉ IP")]
    public string? IpAddress { get; set; }

    [Display(Name = "Thiết bị/Trình duyệt")]
    public string? UserAgent { get; set; }

    [Display(Name = "Cập nhật lần cuối")]
    public DateTime? UpdatedAt { get; set; }

    // Dropdown for status update
    public SelectList? StatusList { get; set; }
}
