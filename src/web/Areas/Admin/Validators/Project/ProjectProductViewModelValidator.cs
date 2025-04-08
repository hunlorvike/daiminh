using FluentValidation;
using web.Areas.Admin.ViewModels.Project;

namespace web.Areas.Admin.Validators.Project;

public class ProjectProductViewModelValidator : AbstractValidator<ProjectProductViewModel>
{
    public ProjectProductViewModelValidator()
    {
        When(x => !x.IsDeleted, () =>
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Sản phẩm không hợp lệ.");
            RuleFor(x => x.OrderIndex).GreaterThanOrEqualTo(0);
        });
    }
}
