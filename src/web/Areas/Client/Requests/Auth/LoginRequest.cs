using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Client.Requests.Auth;

public class LoginRequest
{
    /*[Display(Name = "Email", Prompt = "Nhập tài khoản của bạn")]
    public string? Email { get; set; }*/
    [Display(Name = "Tài khoản", Prompt = "Nhập tài khoản của bạn")]
    public string? Username { get; set; }

    [Display(Name = "Mật khẩu", Prompt = "Nhập mật khẩu của bạn")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        /*RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email đăng nhập không được để trống")
            .EmailAddress().WithMessage("Email không hợp lệ");*/
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Tên đăng nhập không được để trống")
            .Length(3, 50).WithMessage("Tên đăng nhập phải từ 3-50 ký tự");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");
    }
}