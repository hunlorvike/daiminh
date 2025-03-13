using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Auth;

/// <summary>
/// Represents a request for user login.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <example>johndoe</example>
    [Display(Name = "Tài khoản", Prompt = "Nhập tài khoản của bạn")]
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <example>Password123!</example>
    [Display(Name = "Mật khẩu", Prompt = "Nhập mật khẩu của bạn")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}

/// <summary>
/// Validator for <see cref="LoginRequest"/>.
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginRequestValidator"/> class.
    /// </summary>
    public LoginRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Tên đăng nhập không được bỏ trống.")
            .Length(3, 50).WithMessage("Tên đăng nhập phải có từ 3 đến 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới (_).");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được bỏ trống.")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");
    }
}