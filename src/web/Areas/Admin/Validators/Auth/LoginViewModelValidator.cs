using FluentValidation;
using web.Areas.Admin.ViewModels.Account;

namespace web.Areas.Admin.Validators.Auth;

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
    public LoginViewModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Tên đăng nhập không được để trống")
            .MaximumLength(50).WithMessage("Tên đăng nhập không được vượt quá 50 ký tự");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống");
    }
}