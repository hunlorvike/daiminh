using FluentValidation;
using web.Areas.Admin.ViewModels.User;

namespace web.Areas.Admin.Validators.User;

public class UserViewModelValidator : AbstractValidator<UserViewModel>
{
    public UserViewModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Vui lòng nhập tên đăng nhập")
            .MaximumLength(50).WithMessage("Tên đăng nhập không được vượt quá 50 ký tự")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Vui lòng nhập email")
            .MaximumLength(255).WithMessage("Email không được vượt quá 255 ký tự")
            .EmailAddress().WithMessage("Email không hợp lệ");

        // Password is required for new users
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Vui lòng nhập mật khẩu")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự")
            .When(x => x.Id == 0);

        // Password is optional for existing users, but if provided, it must meet requirements
        RuleFor(x => x.Password)
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự")
            .When(x => x.Id != 0 && !string.IsNullOrEmpty(x.Password));

        // Confirm password must match password
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Xác nhận mật khẩu không khớp")
            .When(x => !string.IsNullOrEmpty(x.Password));
    }
}