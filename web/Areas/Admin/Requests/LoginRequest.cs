using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests;

public class LoginRequest
{
    [Required(ErrorMessage = "Vui lòng nhập tên người dùng.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}