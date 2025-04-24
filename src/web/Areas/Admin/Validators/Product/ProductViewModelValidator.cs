using FluentValidation;
using infrastructure;
using web.Areas.Admin.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.Validators.Shared;

namespace web.Areas.Admin.Validators.Product;

public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
{
    private readonly ApplicationDbContext _context;

    public ProductViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .MustAsync(BeUniqueSlugAsync).WithMessage("Slug này đã tồn tại, vui lòng chọn slug khác.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");

        RuleFor(x => x.ShortDescription)
            .MaximumLength(500).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Manufacturer)
            .MaximumLength(255).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Origin)
            .MaximumLength(100).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Status)
             .IsInEnum().WithMessage("{PropertyName} không hợp lệ.");

        RuleFor(x => x.BrandId)
            .MustAsync(BrandExistsAsync).WithMessage("Thương hiệu đã chọn không tồn tại.")
            .When(x => x.BrandId.HasValue);

        RuleFor(x => x.CategoryId)
            .MustAsync(CategoryExistsAsync).WithMessage("Danh mục đã chọn không tồn tại.")
            .When(x => x.CategoryId.HasValue);

        RuleFor(x => x.Images).NotNull().WithMessage("Phải có ít nhất một ảnh sản phẩm.")
            .Must(list => list == null || list.Count(img => !img._Delete) > 0).WithMessage("Phải có ít nhất một ảnh sản phẩm.")
            .ForEach(image => image.SetValidator(new ProductImageViewModelValidator()));

        RuleFor(x => x.Images)
            .Must(HaveOnlyOneMainImage).WithMessage("Chỉ được chọn một ảnh chính.");

        RuleFor(x => x.SelectedTagIds)
            .MustAsync(AllTagsExistAsync).WithMessage("Có thẻ được chọn không hợp lệ.");

        RuleFor(x => x.SelectedArticleIds)
             .MustAsync(AllArticlesExistAsync).WithMessage("Có bài viết liên quan được chọn không hợp lệ.");

        RuleFor(x => x.Seo).SetValidator(new SeoViewModelValidator());
    }

    private async Task<bool> BeUniqueSlugAsync(ProductViewModel viewModel, string slug, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(slug)) return true;

        return !await _context.Set<domain.Entities.Product>()
                              .AnyAsync(p => p.Slug == slug && p.Id != viewModel.Id, cancellationToken);
    }

    private async Task<bool> BrandExistsAsync(ProductViewModel viewModel, int? brandId, CancellationToken cancellationToken)
    {
        if (!brandId.HasValue) return true;

        return await _context.Set<domain.Entities.Brand>()
                             .AnyAsync(b => b.Id == brandId.Value, cancellationToken);
    }

    private async Task<bool> CategoryExistsAsync(ProductViewModel viewModel, int? categoryId, CancellationToken cancellationToken)
    {
        if (!categoryId.HasValue) return true;

        return await _context.Set<domain.Entities.Category>()
                             .AnyAsync(c => c.Id == categoryId.Value && c.Type == shared.Enums.CategoryType.Product, cancellationToken);
    }

    private bool HaveOnlyOneMainImage(List<ProductImageViewModel> images)
    {
        if (images == null || images.Count == 0) return true;
        return images.Count(img => img.IsMain && !img._Delete) <= 1;
    }

    private async Task<bool> AllTagsExistAsync(ProductViewModel viewModel, List<int>? tagIds, CancellationToken cancellationToken)
    {
        if (tagIds == null || !tagIds.Any()) return true;

        var distinctTagIds = tagIds.Distinct().ToList();
        var existingTagCount = await _context.Set<domain.Entities.Tag>()
                                            .CountAsync(t => distinctTagIds.Contains(t.Id), cancellationToken);

        return existingTagCount == distinctTagIds.Count;
    }

    private async Task<bool> AllArticlesExistAsync(ProductViewModel viewModel, List<int>? articleIds, CancellationToken cancellationToken)
    {
        if (articleIds == null || !articleIds.Any()) return true;

        var distinctArticleIds = articleIds.Distinct().ToList();
        var existingArticleCount = await _context.Set<domain.Entities.Article>()
                                                .CountAsync(a => distinctArticleIds.Contains(a.Id), cancellationToken);

        return existingArticleCount == distinctArticleIds.Count;
    }
}
