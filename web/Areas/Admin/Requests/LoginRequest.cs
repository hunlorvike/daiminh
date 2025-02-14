using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests;

public class LoginRequest
{
    [Display(Name = "Tài khoản", Prompt = "Nhập tài khoản của bạn")]
    public string? Username { get; set; }

    [Display(Name = "Mật khẩu", Prompt = "Nhập mật khẩu của bạn")]
    public string? Password { get; set; }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Tên đăng nhập không được để trống")
            .Length(3, 50).WithMessage("Tên đăng nhập phải từ 3-50 ký tự");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");
        // .Matches("[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ hoa")
        // .Matches("[a-z]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ thường")
        // .Matches("[0-9]").WithMessage("Mật khẩu phải chứa ít nhất 1 số");
    }
}