using FluentValidation;
using infrastructure;
using web.Areas.Admin.Validators.Shared;
using web.Areas.Admin.ViewModels.Product;

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
            .Must(BeUniqueSlug).WithMessage("Slug này đã tồn tại, vui lòng chọn slug khác.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");

        RuleFor(x => x.ShortDescription)
            .MaximumLength(500).WithMessage("{PropertyName} không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.Manufacturer)
             .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.Manufacturer));

        RuleFor(x => x.Origin)
             .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Origin));

        RuleFor(x => x.Status)
             .IsInEnum().WithMessage("{PropertyName} không hợp lệ.");

        RuleFor(x => x.BrandId)
            .Must((model, brandId) => brandId == null || BrandExists(brandId)).WithMessage("Thương hiệu được chọn không tồn tại.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Vui lòng chọn {PropertyName}.")
            .Must((model, categoryId) => CategoryExists(categoryId)).WithMessage("Danh mục được chọn không tồn tại.");

        RuleForEach(x => x.Images)
            .ChildRules(image =>
            {
                image.RuleFor(img => img.ImageUrl)
                    .NotEmpty().WithMessage("URL Ảnh không được để trống.")
                    .MaximumLength(255).WithMessage("URL Ảnh không được vượt quá {MaxLength} ký tự.");
                image.RuleFor(img => img.ThumbnailUrl)
                     .MaximumLength(255).When(img => !string.IsNullOrEmpty(img.ThumbnailUrl)).WithMessage("URL Thumbnail không được vượt quá {MaxLength} ký tự.");
                image.RuleFor(img => img.AltText)
                     .MaximumLength(255).When(img => !string.IsNullOrEmpty(img.AltText)).WithMessage("Alt Text không được vượt quá {MaxLength} ký tự.");
                image.RuleFor(img => img.Title)
                     .MaximumLength(255).When(img => !string.IsNullOrEmpty(img.Title)).WithMessage("Tiêu đề ảnh không được vượt quá {MaxLength} ký tự.");
                image.RuleFor(img => img.OrderIndex)
                     .GreaterThanOrEqualTo(0).WithMessage("Thứ tự ảnh phải là số không âm.");
            });

        When(x => x.Images != null && x.Images.Any(img => !img.IsDeleted), () =>
        {
            RuleFor(x => x.Images)
               .Must(images => images.Count(img => !img.IsDeleted && img.IsMain) == 1)
               .WithMessage("Phải có chính xác một ảnh được đánh dấu là ảnh chính.");
        });


        RuleFor(x => x.Seo).SetValidator(new SeoViewModelValidator());
    }

    private bool BeUniqueSlug(ProductViewModel viewModel, string slug)
    {
        return !_context.Set<domain.Entities.Product>()
                              .Any(p => p.Slug == slug && p.Id != viewModel.Id);
    }

    private bool CategoryExists(int? categoryId)
    {
        if (categoryId == null) return false;

        return _context.Set<domain.Entities.Category>()
                       .Any(c => c.Id == categoryId && c.Type == shared.Enums.CategoryType.Product);
    }

    private bool BrandExists(int? brandId)
    {
        if (brandId == null) return true;

        return _context.Set<domain.Entities.Brand>()
                       .Any(b => b.Id == brandId);
    }
}
