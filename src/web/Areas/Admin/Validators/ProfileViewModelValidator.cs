using FluentValidation;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class ProfileViewModelValidator : AbstractValidator<ProfileViewModel>
{
    public ProfileViewModelValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ và tên không được để trống.")
            .MaximumLength(100).WithMessage("Họ và tên không được vượt quá 100 ký tự.");
    }
}