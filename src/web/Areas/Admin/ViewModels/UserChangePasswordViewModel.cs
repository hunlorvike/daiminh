using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class UserChangePasswordViewModel
{
    public int UserId { get; set; }

    [Display(Name = "Mật khẩu mới", Prompt = "Nhập mật khẩu mới")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Display(Name = "Xác nhận mật khẩu mới", Prompt = "Nhập lại mật khẩu mới")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu mới không khớp.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
