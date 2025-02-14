using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests;

public class RegisterRequest
{
    [Required] public string Username { get; set; } = string.Empty;

    [Required] public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}