using FluentValidation;
using web.Areas.Admin.ViewModels.Project;

namespace web.Areas.Admin.Validators.Project;

public class ProjectViewModelValidator : AbstractValidator<ProjectViewModel>
{
    private readonly IList<string> _validFrequencies = new List<string> { "always", "hourly", "daily", "weekly", "monthly", "yearly", "never" };

    public ProjectViewModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(255).Matches("^[a-z0-9-]+$");
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.ShortDescription).MaximumLength(500);
        RuleFor(x => x.Client).MaximumLength(255);
        RuleFor(x => x.Location).MaximumLength(255);
        RuleFor(x => x.Area).GreaterThanOrEqualTo(0).When(x => x.Area.HasValue);
        RuleFor(x => x.CompletionDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.StartDate.HasValue && x.CompletionDate.HasValue).WithMessage("Ngày hoàn thành phải sau hoặc bằng ngày bắt đầu.");
        RuleFor(x => x.FeaturedImage).MaximumLength(255);
        RuleFor(x => x.ThumbnailImage).MaximumLength(255);
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.PublishStatus).IsInEnum();

        // Validate collections
        RuleFor(x => x.SelectedCategoryIds).NotEmpty().WithMessage("Chọn ít nhất một danh mục.");
        RuleForEach(x => x.Images).SetValidator(new ProjectImageViewModelValidator());
        RuleForEach(x => x.ProjectProducts).SetValidator(new ProjectProductViewModelValidator());

        // SEO Fields Validation
        RuleFor(x => x.MetaTitle).MaximumLength(100);
        RuleFor(x => x.MetaDescription).MaximumLength(300);
        RuleFor(x => x.MetaKeywords).MaximumLength(200);
        RuleFor(x => x.CanonicalUrl).MaximumLength(255).Matches(@"^(https?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$").When(x => !string.IsNullOrWhiteSpace(x.CanonicalUrl));
        RuleFor(x => x.OgTitle).MaximumLength(100);
        RuleFor(x => x.OgDescription).MaximumLength(300);
        RuleFor(x => x.OgImage).MaximumLength(255).Matches(@"^(https?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$").When(x => !string.IsNullOrWhiteSpace(x.OgImage));
        RuleFor(x => x.OgType).MaximumLength(50);
        RuleFor(x => x.TwitterTitle).MaximumLength(100);
        RuleFor(x => x.TwitterDescription).MaximumLength(300);
        RuleFor(x => x.TwitterImage).MaximumLength(255).Matches(@"^(https?://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$").When(x => !string.IsNullOrWhiteSpace(x.TwitterImage));
        RuleFor(x => x.TwitterCard).MaximumLength(50);
        RuleFor(x => x.SitemapPriority).InclusiveBetween(0.0, 1.0);
        RuleFor(x => x.SitemapChangeFrequency)
            .NotEmpty()
            .Must(f => _validFrequencies.Contains(f?.ToLowerInvariant() ?? string.Empty))
            .WithMessage("Tần suất cập nhật sitemap không hợp lệ.");
    }
}