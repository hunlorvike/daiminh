using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Project;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Validators.Project;
public class ProjectViewModelValidator : AbstractValidator<ProjectViewModel>
{
    private readonly ApplicationDbContext _context;

    public ProjectViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name).NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.").MaximumLength(255);
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255)
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ chứa chữ thường, số, dấu gạch ngang.")
            .Must(BeUniqueSlug).WithMessage("{PropertyName} này đã tồn tại.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");
        RuleFor(x => x.ShortDescription).MaximumLength(500);
        RuleFor(x => x.Client).MaximumLength(255);
        RuleFor(x => x.Location).MaximumLength(255);
        RuleFor(x => x.Area).GreaterThanOrEqualTo(0).When(x => x.Area.HasValue);
        RuleFor(x => x.FeaturedImage).MaximumLength(2048);
        RuleFor(x => x.ThumbnailImage).MaximumLength(2048);
        RuleFor(x => x.Status).IsInEnum().WithMessage("{PropertyName} không hợp lệ.");
        RuleFor(x => x.PublishStatus).IsInEnum().WithMessage("{PropertyName} không hợp lệ.");

        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Vui lòng chọn một {PropertyName}.");

        RuleForEach(x => x.Images).SetValidator(new ProjectImageViewModelValidator());

        Include(new SeoPropertiesValidator<ProjectViewModel>());
    }

    private bool BeUniqueSlug(ProjectViewModel viewModel, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return true;
        return !_context.Set<domain.Entities.Project>()
                               .Any(p => p.Slug.ToLower() == slug.ToLower() && p.Id != viewModel.Id);
    }
}