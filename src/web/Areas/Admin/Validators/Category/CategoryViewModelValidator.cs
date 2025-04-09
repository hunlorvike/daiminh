using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels.Category;

namespace web.Areas.Admin.Validators.Category;

public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
{
    private readonly ApplicationDbContext _context;
    private readonly IList<string> _validFrequencies = new List<string> { "always", "hourly", "daily", "weekly", "monthly", "yearly", "never" };

    public CategoryViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên danh mục.")
            .MaximumLength(100).WithMessage("Tên danh mục không được vượt quá 100 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug.")
            .MaximumLength(100).WithMessage("Slug không được vượt quá 100 ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự.");

        RuleFor(x => x.Icon)
            .MaximumLength(50).WithMessage("Icon không được vượt quá 50 ký tự.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(255).WithMessage("Đường dẫn ảnh không được vượt quá 255 ký tự.");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Thứ tự phải là số không âm.");

        // ParentId check: Ensure it's not the same as the current ID (for Edit scenario)
        RuleFor(x => x.ParentId)
            .NotEqual(x => x.Id).When(x => x.ParentId.HasValue && x.Id > 0) // Only check on Edit when ParentId is set
            .WithMessage("Danh mục không thể là con của chính nó.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Loại danh mục không hợp lệ.");

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
        // SchemaMarkup and BreadcrumbJson are text, no length limit here unless DB has one

        RuleFor(x => x.SitemapPriority).InclusiveBetween(0.0, 1.0);
        RuleFor(x => x.SitemapChangeFrequency)
            .NotEmpty()
            .Must(f => _validFrequencies.Contains(f?.ToLowerInvariant() ?? string.Empty))
            .WithMessage("Tần suất cập nhật sitemap không hợp lệ.");
    }
}