using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên người dùng.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}