using System.ComponentModel.DataAnnotations;
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace web.Areas.Client.Requests.Auth;

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
    private readonly ApplicationDbContext _context;
    private User? _cachedUser;
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginRequestValidator"/> class.
    /// </summary>
    public LoginRequestValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(request => request.Username)
           .NotEmpty().WithMessage("Tên đăng nhập không được bỏ trống.")
           .Length(3, 50).WithMessage("Tên đăng nhập phải có từ 3 đến 50 ký tự.")
           .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới (_).")
           .MustAsync(async (username, cancellation) =>
           {
               _cachedUser = await _context.Users
                   .FirstOrDefaultAsync(u => u.Username == username, cancellation);
               return _cachedUser != null;
           }).WithMessage("Tên đăng nhập không tồn tại trong hệ thống.");

        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Mật khẩu không được bỏ trống.")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.")
            .Must(password =>
            {
                if (_cachedUser == null) return true;
                return BC.Verify(password, _cachedUser.PasswordHash);
            }).WithMessage("Mật khẩu không chính xác.");
    }
}