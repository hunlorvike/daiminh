using domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class UserViewModelValidator : AbstractValidator<UserViewModel>
{
    private readonly UserManager<User> _userManager;

    public UserViewModelValidator(UserManager<User> userManager)
    {
        _userManager = userManager;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .EmailAddress().WithMessage("Địa chỉ {PropertyName} không hợp lệ.")
            .MaximumLength(256).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .MustAsync(BeUniqueEmail).WithMessage("Địa chỉ Email đã tồn tại. Vui lòng chọn địa chỉ khác.");

        RuleFor(x => x.FullName)
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        When(x => x.Id == 0 || x.HasPassword == false, () =>
        {
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
                .MinimumLength(6).WithMessage("{PropertyName} phải có ít nhất {MinLength} ký tự.")
                .Matches("[A-Z]").WithMessage("{PropertyName} phải chứa ít nhất một chữ cái in hoa.")
                .Matches("[a-z]").WithMessage("{PropertyName} phải chứa ít nhất một chữ cái thường.")
                .Matches("[0-9]").WithMessage("{PropertyName} phải chứa ít nhất một chữ số.")
                .Matches("[^a-zA-Z0-9]").WithMessage("{PropertyName} phải chứa ít nhất một ký tự đặc biệt.")
                .Equal(x => x.ConfirmPassword).WithMessage("Mật khẩu và xác nhận mật khẩu không khớp.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");
        });

        When(x => x.Id != 0 && !string.IsNullOrEmpty(x.Password), () =>
        {
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("{PropertyName} phải có ít nhất {MinLength} ký tự.")
                .Matches("[A-Z]").WithMessage("{PropertyName} phải chứa ít nhất một chữ cái in hoa.")
                .Matches("[a-z]").WithMessage("{PropertyName} phải chứa ít nhất một chữ cái thường.")
                .Matches("[0-9]").WithMessage("{PropertyName} phải chứa ít nhất một chữ số.")
                .Matches("[^a-zA-Z0-9]").WithMessage("{PropertyName} phải chứa ít nhất một ký tự đặc biệt.")
                .Equal(x => x.ConfirmPassword).WithMessage("Mật khẩu mới và xác nhận mật khẩu không khớp.");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");
        });
    }

    private async Task<bool> BeUniqueEmail(UserViewModel viewModel, string email, ValidationContext<UserViewModel> context, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user == null || user.Id == viewModel.Id;
    }
}