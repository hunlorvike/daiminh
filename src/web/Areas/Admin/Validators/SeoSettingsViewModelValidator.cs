using FluentValidation;
using web.Areas.Admin.ViewModels.Seo;

namespace web.Areas.Admin.Validators;

public class SeoSettingsViewModelValidator : AbstractValidator<SeoSettingsViewModel>
{
    public SeoSettingsViewModelValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Khóa cài đặt không được để trống")
            .MaximumLength(100).WithMessage("Khóa cài đặt không được vượt quá 100 ký tự");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự");
    }
}