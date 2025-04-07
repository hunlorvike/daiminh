using FluentValidation;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Validators.Media;

public class MediaFileEditViewModelValidator : AbstractValidator<MediaFileEditViewModel>
{
    public MediaFileEditViewModelValidator()
    {
        RuleFor(x => x.Id)
           .NotEmpty().WithMessage("ID file không được rỗng.");

        RuleFor(x => x.AltText)
            .MaximumLength(255).WithMessage("Alt Text không được vượt quá 255 ký tự.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự.");
    }
}
