using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class UserEditViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tên đăng nhập", Prompt = "Nhập tên đăng nhập")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(50, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression("^[a-zA-Z0-9_.-]+$", ErrorMessage = "{0} chỉ chấp nhận chữ cái, số, dấu gạch dưới, dấu chấm, dấu gạch ngang.")]
    public string UserName { get; set; } = string.Empty;

    [Display(Name = "Địa chỉ Email", Prompt = "Nhập địa chỉ email")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Họ và Tên", Prompt = "Nhập họ và tên đầy đủ (không bắt buộc)")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? FullName { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [Display(Name = "Vai trò")]
    public List<string> SelectedRoles { get; set; } = new List<string>();
    public List<SelectListItem> AllRoles { get; set; } = new List<SelectListItem>();
}