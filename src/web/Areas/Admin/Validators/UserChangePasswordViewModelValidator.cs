using FluentValidation;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class UserChangePasswordViewModelValidator : AbstractValidator<UserChangePasswordViewModel>
{
    public UserChangePasswordViewModelValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MinimumLength(6).WithMessage("{PropertyName} phải có ít nhất {MinLength} ký tự.") // Ví dụ
            .Matches("[A-Z]").WithMessage("{PropertyName} phải chứa ít nhất một chữ cái in hoa.") // Ví dụ
            .Matches("[a-z]").WithMessage("{PropertyName} phải chứa ít nhất một chữ cái thường.") // Ví dụ
            .Matches("[0-9]").WithMessage("{PropertyName} phải chứa ít nhất một chữ số.") // Ví dụ
            .Matches("[^a-zA-Z0-9]").WithMessage("{PropertyName} phải chứa ít nhất một ký tự đặc biệt.") // Ví dụ
            .Equal(x => x.ConfirmNewPassword).WithMessage("Mật khẩu mới và xác nhận mật khẩu mới không khớp.");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");
    }
}