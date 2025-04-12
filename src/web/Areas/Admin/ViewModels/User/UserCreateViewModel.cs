using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.User;

public class UserCreateViewModel
{
    [Display(Name = "Tên đăng nhập", Prompt = "Nhập tên đăng nhập")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(50, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    [RegularExpression("^[a-zA-Z0-9_.-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái, số, dấu gạch dưới, dấu chấm, dấu gạch ngang")]
    public string Username { get; set; } = string.Empty;

    [Display(Name = "Địa chỉ Email", Prompt = "Nhập địa chỉ email")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    [EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Mật khẩu", Prompt = "Nhập mật khẩu (ít nhất 6 ký tự)")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "{0} phải có ít nhất {1} ký tự")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Xác nhận mật khẩu", Prompt = "Nhập lại mật khẩu")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Họ và Tên", Prompt = "Nhập họ và tên")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? FullName { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;
}
