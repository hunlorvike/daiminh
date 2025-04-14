using FluentValidation;
using web.Areas.Admin.ViewModels.Project;
namespace web.Areas.Admin.Validators.Project;
public class ProjectImageViewModelValidator : AbstractValidator<ProjectImageViewModel>
{
    public ProjectImageViewModelValidator()
    {
        When(x => !x.IsDeleted, () =>
        {
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("{PropertyName} không được rỗng.").MaximumLength(2048);
            RuleFor(x => x.ThumbnailUrl).MaximumLength(2048);
            RuleFor(x => x.AltText).MaximumLength(255);
            RuleFor(x => x.Title).MaximumLength(255);
            RuleFor(x => x.OrderIndex).GreaterThanOrEqualTo(0);
        });
    }
}