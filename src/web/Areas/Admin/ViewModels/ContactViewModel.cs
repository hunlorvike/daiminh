using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;
namespace web.Areas.Admin.ViewModels;

public class ContactViewModel
{
    public int Id { get; set; }

    [Display(Name = "Người gửi")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Số điện thoại")]
    public string? Phone { get; set; }

    [Display(Name = "Chủ đề")]
    public string Subject { get; set; } = string.Empty;

    [Display(Name = "Nội dung lời nhắn")]
    public string Message { get; set; } = string.Empty;

    [Display(Name = "Ngày gửi")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Địa chỉ IP")]
    public string? IpAddress { get; set; }

    [Display(Name = "Trình duyệt (User Agent)")]
    public string? UserAgent { get; set; }

    [Display(Name = "Trạng thái")]
    [Required(ErrorMessage = "Vui lòng chọn trạng thái.")]
    public ContactStatus Status { get; set; } = ContactStatus.New;

    [Display(Name = "Ghi chú nội bộ", Prompt = "Thêm ghi chú cho liên hệ này (chỉ quản trị viên thấy)")]
    public string? AdminNotes { get; set; }

    public List<SelectListItem>? StatusOptions { get; set; }
}