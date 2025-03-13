using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Auth;

/// <summary>
/// Represents a request for user registration.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <example>johndoe</example>
    [Display(Name = "Tài khoản", Prompt = "Nhập tài khoản của bạn")]
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    /// <example>john.doe@example.com</example>
    [Display(Name = "Địa chỉ email", Prompt = "Nhập email của bạn")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <example>Password123!</example>
    [Display(Name = "Mật khẩu", Prompt = "Nhập mật khẩu của bạn")]
    public string? Password { get; set; }
}

/// <summary>
/// Validator for <see cref="RegisterRequest"/>.
/// </summary>
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterRequestValidator"/> class.
    /// </summary>
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Tên đăng nhập không được bỏ trống.")
            .Length(3, 50).WithMessage("Tên đăng nhập phải có từ 3 đến 50 ký tự.")
             .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới (_).");

        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Địa chỉ email không được bỏ trống.")
            .EmailAddress().WithMessage("Địa chỉ email không hợp lệ. Vui lòng nhập một địa chỉ email hợp lệ (ví dụ: ten@example.com).");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được bỏ trống.")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.")
            .Matches("[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất một chữ cái viết hoa.")
            .Matches("[a-z]").WithMessage("Mật khẩu phải chứa ít nhất một chữ cái viết thường.")
            .Matches("[0-9]").WithMessage("Mật khẩu phải chứa ít nhất một chữ số.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Mật khẩu phải chứa ít nhất một ký tự đặc biệt (ví dụ: @, #, $,...).");
    }
}