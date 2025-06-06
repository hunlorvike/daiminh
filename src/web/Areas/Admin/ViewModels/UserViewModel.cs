using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }

    [Display(Name = "Email", Prompt = "Nhập địa chỉ email")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [EmailAddress(ErrorMessage = "Địa chỉ {0} không hợp lệ.")]
    [MaxLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Họ và tên", Prompt = "Nhập họ và tên đầy đủ")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? FullName { get; set; }

    [Display(Name = "Mật khẩu", Prompt = "Nhập mật khẩu")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Xác nhận mật khẩu", Prompt = "Nhập lại mật khẩu")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
    public string? ConfirmPassword { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; }

    [Display(Name = "Vai trò được gán")]
    public List<int> SelectedRoleIds { get; set; } = new List<int>();
    public List<SelectListItem>? AvailableRoles { get; set; }

    [Display(Name = "Quyền hạn trực tiếp")]
    public List<int> SelectedClaimDefinitionIds { get; set; } = new List<int>();
    public List<SelectListItem>? AvailableClaimDefinitions { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }
    public bool HasPassword { get; set; }
    public bool EmailConfirmed { get; set; }
}
