using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Client.Requests.Auth;

public class RegisterRequest
{
    [Display(Name = "Tài khoản", Prompt = "Nhập tài khoản của bạn")]
    public string? Username { get; set; }

    [Display(Name = "Địa chỉ email", Prompt = "Nhập email của bạn")]
    public string? Email { get; set; }

    [Display(Name = "Mật khẩu", Prompt = "Nhập mật khẩu của bạn")]
    public string? Password { get; set; }

    [Display(Name = "Xác nhận mật khẩu", Prompt = "Nhập lại mật khẩu")]
    public string? PasswordConfirm { get; set; }
}

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Tên người dùng không được để trống")
            .Length(3, 50).WithMessage("Tên người dùng phải từ 3-50 ký tự");

        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Địa chỉ email không được để trống")
            .EmailAddress().WithMessage("Địa chỉ email không hợp lệ");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự")
            .Matches("[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ hoa")
            .Matches("[a-z]").WithMessage("Mật khẩu phải chứa ít nhất 1 chữ thường")
            .Matches("[0-9]").WithMessage("Mật khẩu phải chứa ít nhất 1 số");

        RuleFor(x => x.PasswordConfirm)
            .NotEmpty().WithMessage("Vui lòng xác nhận mật khẩu")
            .Equal(x => x.Password).WithMessage("Mật khẩu xác nhận không khớp");
    }
}