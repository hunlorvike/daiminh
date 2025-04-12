using FluentValidation;
using web.Areas.Admin.ViewModels.Setting;

namespace web.Areas.Admin.Validators.Setting;

public class SettingUpdateViewModelValidator : AbstractValidator<SettingUpdateViewModel>
{
    public SettingUpdateViewModelValidator()
    {
        RuleFor(x => x.Settings)
            .NotEmpty().WithMessage("Danh sách cài đặt không được rỗng.");

        RuleForEach(x => x.Settings).SetValidator(new SettingValidator());
    }
}
