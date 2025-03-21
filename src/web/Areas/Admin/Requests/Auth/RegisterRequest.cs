using System.ComponentModel.DataAnnotations;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

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
    [Display(Name = "Tài khoản", Prompt = "Nhập tài khoản")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới (_).")]
    [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống.")]

    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    /// <example>john.doe@example.com</example>
    [Display(Name = "Email", Prompt = "Nhập email")]
    [Required(ErrorMessage = "Địa chỉ email không được bỏ trống.")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <example>Password123!</example>
    [Display(Name = "Mật khẩu", Prompt = "Nhập mật khẩu")]
    [Required(ErrorMessage = "Mật khẩu không được bỏ trống.")]
    public string? Password { get; set; }
}

/// <summary>
/// Validator for <see cref="RegisterRequest"/>.
/// </summary>
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    private readonly ApplicationDbContext _context;
    private User? _cachedUser;

    public RegisterRequestValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Tên đăng nhập không được bỏ trống.")
            .Length(3, 50).WithMessage("Tên đăng nhập phải có từ 3 đến 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới (_).")
            .MustAsync(async (request, username, cancellation) =>
            {
                _cachedUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username || u.Email == request.Email, cancellation);

                return _cachedUser == null || _cachedUser.Username != username;
            }).WithMessage("Tên đăng nhập này đã được sử dụng. Vui lòng chọn một tên khác.");

        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Địa chỉ email không được bỏ trống.")
            .EmailAddress().WithMessage("Địa chỉ email không hợp lệ. Vui lòng nhập một địa chỉ email hợp lệ (ví dụ: ten@example.com).")
            .Must((request, email) =>
            {
                if (_cachedUser == null) return true;
                return _cachedUser.Email != email;
            }).WithMessage("Địa chỉ email này đã được đăng ký. Vui lòng sử dụng một địa chỉ email khác.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được bỏ trống.")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.")
            .Matches("[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất một chữ cái viết hoa.")
            .Matches("[a-z]").WithMessage("Mật khẩu phải chứa ít nhất một chữ cái viết thường.")
            .Matches("[0-9]").WithMessage("Mật khẩu phải chứa ít nhất một chữ số.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Mật khẩu phải chứa ít nhất một ký tự đặc biệt (ví dụ: @, #, $,...).");
    }
}
