using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class ProfileViewModel
{
    [Display(Name = "Email (Tên đăng nhập)")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Họ và tên")]
    [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;
}