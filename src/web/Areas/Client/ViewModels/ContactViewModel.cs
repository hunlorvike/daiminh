using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Client.ViewModels;

public class ContactViewModel
{
    [Display(Name = "Họ và tên", Prompt = "Nhập họ và tên")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Email", Prompt = "Nhập địa chỉ email")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Số điện thoại", Prompt = "Nhập số điện thoại")]
    public string? Phone { get; set; }

    [Display(Name = "Chủ đề", Prompt = "Nhập chủ đề")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    public string Subject { get; set; } = string.Empty;

    [Display(Name = "Nội dung", Prompt = "Nhập nội dung")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    public string Message { get; set; } = string.Empty;
    public List<SelectListItem> SubjectOptions { get; set; } = [];
}