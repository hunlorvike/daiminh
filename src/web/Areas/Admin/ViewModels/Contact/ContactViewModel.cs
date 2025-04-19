using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
namespace web.Areas.Admin.ViewModels.Contact;

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

    [Display(Name = "Tên công ty")]
    public string? CompanyName { get; set; }

    [Display(Name = "Chi tiết dự án")]
    public string? ProjectDetails { get; set; }

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