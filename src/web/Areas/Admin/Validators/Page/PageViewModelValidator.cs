// Path: web.Areas.Admin.Validators.Page
using FluentValidation;
using infrastructure; // Assuming your DbContext is in infrastructure
using web.Areas.Admin.ViewModels.Page;

namespace web.Areas.Admin.Validators.Page;

public class PageViewModelValidator : AbstractValidator<PageViewModel>
{
    private readonly ApplicationDbContext _context;

    public PageViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .Must(BeUniqueSlug).WithMessage("{PropertyName} này đã tồn tại, vui lòng chọn slug khác.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");

        // Validation for inherited SeoViewModel properties - manually add rules here
        RuleFor(x => x.MetaTitle).MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");
        RuleFor(x => x.MetaDescription).MaximumLength(300).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");
        RuleFor(x => x.MetaKeywords).MaximumLength(200).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");
        RuleFor(x => x.CanonicalUrl)
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches(@"^(https?://|/).*$").When(x => !string.IsNullOrWhiteSpace(x.CanonicalUrl)).WithMessage("{PropertyName} phải là một URL hợp lệ (http, https hoặc tương đối /).");

        RuleFor(x => x.OgTitle).MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");
        RuleFor(x => x.OgDescription).MaximumLength(300).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");
        RuleFor(x => x.OgImage)
             .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
             .Matches(@"^(https?://|/).*$").When(x => !string.IsNullOrWhiteSpace(x.OgImage)).WithMessage("{PropertyName} phải là một URL hợp lệ (http, https hoặc tương đối /).");


        RuleFor(x => x.TwitterTitle).MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");
        RuleFor(x => x.TwitterDescription).MaximumLength(300).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");
         RuleFor(x => x.TwitterImage)
             .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
             .Matches(@"^(https?://|/).*$").When(x => !string.IsNullOrWhiteSpace(x.TwitterImage)).WithMessage("{PropertyName} phải là một URL hợp lệ (http, https hoặc tương đối /).");

        RuleFor(x => x.SitemapPriority)
            .InclusiveBetween(0.0, 1.0).When(x => x.SitemapPriority.HasValue).WithMessage("{PropertyName} phải nằm trong khoảng từ {From} đến {To}.");

        RuleFor(x => x.SitemapChangeFrequency).MaximumLength(20);

        // SchemaMarkup doesn't typically have length limits for validation, just format/content.
    }

    private bool BeUniqueSlug(PageViewModel viewModel, string slug)
    {
        return !_context.Set<domain.Entities.Page>()
                              .Any(p => p.Slug == slug && p.Id != viewModel.Id);
    }
}