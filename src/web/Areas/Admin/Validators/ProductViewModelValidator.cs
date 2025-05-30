// src/web/Areas/Admin/Validators/ProductViewModelValidator.cs
using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;
using web.Areas.Admin.Validators.Shared;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
{
    private readonly ApplicationDbContext _context;

    public ProductViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên sản phẩm không được để trống.")
            .MaximumLength(255).WithMessage("Tên sản phẩm không được vượt quá 255 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug không được để trống.")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .MustAsync(BeUniqueSlug).WithMessage("Slug này đã tồn tại. Vui lòng chọn slug khác.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Mô tả chi tiết không được để trống.");

        RuleFor(x => x.ShortDescription)
            .MaximumLength(500).WithMessage("Mô tả ngắn không được vượt quá 500 ký tự.");

        RuleFor(x => x.Manufacturer)
            .MaximumLength(255).WithMessage("Nhà sản xuất không được vượt quá 255 ký tự.");

        RuleFor(x => x.Origin)
            .MaximumLength(100).WithMessage("Xuất xứ không được vượt quá 100 ký tự.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Trạng thái xuất bản không hợp lệ.");

        RuleFor(x => x.BrandId)
            .MustAsync(BrandExists).When(x => x.BrandId.HasValue).WithMessage("Thương hiệu không hợp lệ.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Vui lòng chọn danh mục sản phẩm.")
            .MustAsync(CategoryExists).WithMessage("Danh mục sản phẩm không hợp lệ.");

        RuleForEach(x => x.Images).SetValidator(new ProductImageViewModelValidator());

        RuleFor(x => x.Images)
            .Must(images => images != null && images.Any(img => img.IsMain))
            .WithMessage("Phải có ít nhất một ảnh được chọn làm ảnh chính.")
            .When(x => x.Images != null && x.Images.Any());

        Include(new SeoViewModelValidator());
    }

    private async Task<bool> BeUniqueSlug(ProductViewModel viewModel, string slug, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(slug)) return true;
        return !await _context.Set<Product>()
                              .AnyAsync(p => p.Slug == slug && p.Id != viewModel.Id, cancellationToken);
    }

    private async Task<bool> BrandExists(int? brandId, CancellationToken cancellationToken)
    {
        if (!brandId.HasValue) return true;
        return await _context.Set<Brand>().AnyAsync(b => b.Id == brandId.Value, cancellationToken);
    }

    private async Task<bool> CategoryExists(int? categoryId, CancellationToken cancellationToken)
    {
        if (!categoryId.HasValue) return false;
        return await _context.Set<Category>()
                             .AnyAsync(c => c.Id == categoryId.Value && c.Type == CategoryType.Product, cancellationToken);
    }
}

public class ProductImageViewModelValidator : AbstractValidator<ProductImageViewModel>
{
    public ProductImageViewModelValidator()
    {
        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("URL hình ảnh không được để trống.")
            .MaximumLength(255).WithMessage("URL hình ảnh không được vượt quá 255 ký tự.");
    }
}