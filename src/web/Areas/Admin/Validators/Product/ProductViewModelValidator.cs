using FluentValidation;
using shared.Enums;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Validators.Product;

public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
{
    private readonly IList<string> _validFrequencies = new List<string> { "always", "hourly", "daily", "weekly", "monthly", "yearly", "never" };

    public ProductViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên sản phẩm.")
            .MaximumLength(255);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug.")
            .MaximumLength(255)
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Vui lòng nhập mô tả chi tiết."); // Usually required
        RuleFor(x => x.ShortDescription).MaximumLength(500);
        RuleFor(x => x.Manufacturer).MaximumLength(255);
        RuleFor(x => x.Origin).MaximumLength(100);
        // Other text fields: Add length limits if needed, default is Text type

        RuleFor(x => x.ProductTypeId).NotEmpty().WithMessage("Vui lòng chọn loại sản phẩm.");
        // BrandId is nullable, no NotEmpty needed

        RuleFor(x => x.Status).IsInEnum().WithMessage("Trạng thái không hợp lệ.");

        // Validate collections
        //RuleFor(x => x.SelectedCategoryIds)
        //    .NotEmpty().WithMessage("Vui lòng chọn ít nhất một danh mục.")
        //    .When(x => x.Type == CategoryType.Product); // Assuming Categories are required for Products

        // Apply validators to nested collections, skipping deleted items
        RuleForEach(x => x.Images).SetValidator(new ProductImageViewModelValidator());
        RuleForEach(x => x.Variants).SetValidator(new ProductVariantViewModelValidator());

        // --- SEO Fields Validation (Copied from Brand/Category) ---
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
