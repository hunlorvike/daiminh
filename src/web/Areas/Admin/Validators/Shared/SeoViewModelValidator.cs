using FluentValidation;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.Validators.Shared;

public class SeoViewModelValidator : AbstractValidator<SeoViewModel>
{
    private static readonly string[] _validChangeFrequencies = { "always", "hourly", "daily", "weekly", "monthly", "yearly", "never" };
    private static readonly string[] _validTwitterCards = { "summary", "summary_large_image", "app", "player" };

    public SeoViewModelValidator()
    {
        RuleFor(x => x.MetaTitle)
          .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá 100 ký tự.");

        RuleFor(x => x.MetaDescription)
            .MaximumLength(300).WithMessage("{PropertyName} không được vượt quá 300 ký tự.");

        RuleFor(x => x.MetaKeywords)
            .MaximumLength(200).WithMessage("{PropertyName} không được vượt quá 200 ký tự.");

        RuleFor(x => x.CanonicalUrl)
             .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá 255 ký tự.")
             .Matches(@"^(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$").WithMessage("{PropertyName} phải là một URL hợp lệ.")
             .When(x => !string.IsNullOrEmpty(x.CanonicalUrl));

        RuleFor(x => x.OgTitle)
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá 100 ký tự.");

        RuleFor(x => x.OgDescription)
            .MaximumLength(300).WithMessage("{PropertyName} không được vượt quá 300 ký tự.");

        RuleFor(x => x.OgImage)
            .MaximumLength(255).WithMessage("{PropertyName} (URL/Path) không được vượt quá 255 ký tự.");

        RuleFor(x => x.OgType)
            .MaximumLength(50).WithMessage("{PropertyName} không được vượt quá 50 ký tự.");

        RuleFor(x => x.TwitterTitle)
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá 100 ký tự.");

        RuleFor(x => x.TwitterDescription)
            .MaximumLength(300).WithMessage("{PropertyName} không được vượt quá 300 ký tự.");

        RuleFor(x => x.TwitterImage)
            .MaximumLength(255).WithMessage("{PropertyName} (URL/Path) không được vượt quá 255 ký tự.");

        RuleFor(x => x.TwitterCard)
            .MaximumLength(50).WithMessage("{PropertyName} không được vượt quá 50 ký tự.")
            .Must(card => string.IsNullOrEmpty(card) || _validTwitterCards.Contains(card.ToLowerInvariant()))
            .WithMessage("{PropertyName} phải là một loại Twitter Card hợp lệ (vd: summary, summary_large_image).");

        RuleFor(x => x.SitemapPriority)
             .InclusiveBetween(0.0, 1.0).WithMessage("{PropertyName} phải nằm trong khoảng 0.0 đến 1.0.");

        RuleFor(x => x.SitemapChangeFrequency)
            .NotEmpty().WithMessage("{PropertyName} không được để trống.")
            .Must(freq => freq != null && _validChangeFrequencies.Contains(freq.ToLowerInvariant()))
            .WithMessage("{PropertyName} phải là một giá trị tần suất hợp lệ (vd: daily, weekly, monthly).")
            .When(x => x.SitemapChangeFrequency != null);
    }
}