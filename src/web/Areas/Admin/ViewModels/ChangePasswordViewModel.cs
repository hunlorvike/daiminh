using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class ChangePasswordViewModel
{
    [Display(Name = "Mật khẩu hiện tại")]
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại.")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Display(Name = "Mật khẩu mới")]
    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Display(Name = "Xác nhận mật khẩu mới")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}