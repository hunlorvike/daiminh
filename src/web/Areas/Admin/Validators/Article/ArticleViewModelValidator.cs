using FluentValidation;
using web.Areas.Admin.ViewModels.Article;

namespace web.Areas.Admin.Validators.Article;

public class ArticleViewModelValidator : AbstractValidator<ArticleViewModel>
{
    private readonly IList<string> _validFrequencies = new List<string> { "always", "hourly", "daily", "weekly", "monthly", "yearly", "never" };

    public ArticleViewModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Vui lòng nhập tiêu đề bài viết.")
            .MaximumLength(255);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug.")
            .MaximumLength(255)
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ chứa chữ thường, số, dấu gạch ngang.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Vui lòng nhập nội dung bài viết.");

        RuleFor(x => x.Summary).MaximumLength(500);
        RuleFor(x => x.FeaturedImage).MaximumLength(255);
        RuleFor(x => x.ThumbnailImage).MaximumLength(255);
        RuleFor(x => x.AuthorName).MaximumLength(100);
        RuleFor(x => x.AuthorAvatar).MaximumLength(255);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Status).IsInEnum();

        RuleFor(x => x.SelectedCategoryIds)
            .NotEmpty().WithMessage("Vui lòng chọn ít nhất một danh mục.");

        // --- SEO Fields Validation ---
        RuleFor(x => x.MetaTitle).MaximumLength(100);
        RuleFor(x => x.MetaDescription).MaximumLength(300);
        RuleFor(x => x.MetaKeywords).MaximumLength(200);
        RuleFor(x => x.CanonicalUrl).MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.CanonicalUrl));
        RuleFor(x => x.OgTitle).MaximumLength(100);
        RuleFor(x => x.OgDescription).MaximumLength(300);
        RuleFor(x => x.OgImage).MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.OgImage));
        RuleFor(x => x.OgType).MaximumLength(50);
        RuleFor(x => x.TwitterTitle).MaximumLength(100);
        RuleFor(x => x.TwitterDescription).MaximumLength(300);
        RuleFor(x => x.TwitterImage).MaximumLength(255).When(x => !string.IsNullOrWhiteSpace(x.TwitterImage));
        RuleFor(x => x.TwitterCard).MaximumLength(50);
        RuleFor(x => x.SitemapPriority).InclusiveBetween(0.0, 1.0);
        RuleFor(x => x.SitemapChangeFrequency)
            .NotEmpty()
            .Must(f => _validFrequencies.Contains(f?.ToLowerInvariant() ?? string.Empty))
            .WithMessage("Tần suất cập nhật sitemap không hợp lệ.");
    }
}