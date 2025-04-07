using FluentValidation;
using web.Areas.Admin.ViewModels.Brand;

namespace web.Areas.Admin.Validators.Brand;

public class BrandViewModelValidator : AbstractValidator<BrandViewModel>
{
    private readonly IList<string> _validFrequencies = new List<string> { "always", "hourly", "daily", "weekly", "monthly", "yearly", "never" };

    public BrandViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên thương hiệu.")
            .MaximumLength(255).WithMessage("Tên thương hiệu không được vượt quá 255 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug.")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.");

        // Description dùng text nên không cần MaxLength ở đây

        RuleFor(x => x.LogoUrl)
            .MaximumLength(2048).WithMessage("Đường dẫn logo quá dài.");

        RuleFor(x => x.Website)
            .MaximumLength(255).WithMessage("Website không được vượt quá 255 ký tự.")
            .WithMessage("Địa chỉ website không hợp lệ.")
            .When(x => !string.IsNullOrWhiteSpace(x.Website));

        // SEO Fields Validation
        RuleFor(x => x.MetaTitle).MaximumLength(100);
        RuleFor(x => x.MetaDescription).MaximumLength(300);
        RuleFor(x => x.MetaKeywords).MaximumLength(200);
        RuleFor(x => x.CanonicalUrl).MaximumLength(255)
            .When(x => !string.IsNullOrWhiteSpace(x.CanonicalUrl));
        RuleFor(x => x.OgTitle).MaximumLength(100);
        RuleFor(x => x.OgDescription).MaximumLength(300);
        RuleFor(x => x.OgImage)
            .MaximumLength(255)
            .When(x => !string.IsNullOrWhiteSpace(x.OgImage));
        RuleFor(x => x.OgType).MaximumLength(50);
        RuleFor(x => x.TwitterTitle).MaximumLength(100);
        RuleFor(x => x.TwitterDescription).MaximumLength(300);
        RuleFor(x => x.TwitterImage).MaximumLength(255)
            .When(x => !string.IsNullOrWhiteSpace(x.TwitterImage));
        RuleFor(x => x.TwitterCard).MaximumLength(50);
        RuleFor(x => x.SitemapPriority).InclusiveBetween(0.0, 1.0).WithMessage("Sitemap Priority phải từ 0.0 đến 1.0.");
        RuleFor(x => x.SitemapChangeFrequency)
            .NotEmpty().WithMessage("Vui lòng chọn tần suất cập nhật sitemap.")
            .Must(BeValidFrequency).WithMessage("Tần suất cập nhật sitemap không hợp lệ.");
    }

    private bool BeValidFrequency(string frequency)
    {
        return _validFrequencies.Contains(frequency?.ToLowerInvariant() ?? string.Empty);
    }
}
