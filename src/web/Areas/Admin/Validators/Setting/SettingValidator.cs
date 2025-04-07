using FluentValidation;
using web.Areas.Admin.ViewModels.Setting;

namespace web.Areas.Admin.Validators.Setting;

public class SettingValidator : AbstractValidator<SettingViewModel>
{
    public SettingValidator()
    {
        RuleFor(x => x.Value)
            .EmailAddress().WithMessage("Email không hợp lệ.")
            .When(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase));

        RuleFor(x => x.Value)
            .Must(BeValidNumber).WithMessage("Phải là một số hợp lệ.")
            .When(x => x.Type.Equals("Number", StringComparison.OrdinalIgnoreCase));
    }

    private bool BeValidNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return true;
        return double.TryParse(value, out _);
    }
}

public class SettingUpdateViewModelValidator : AbstractValidator<SettingUpdateViewModel>
{
    public SettingUpdateViewModelValidator()
    {
        RuleFor(x => x.Settings)
            .NotEmpty().WithMessage("Danh sách cài đặt không được rỗng.");

        RuleForEach(x => x.Settings).SetValidator(new SettingValidator());
    }
}