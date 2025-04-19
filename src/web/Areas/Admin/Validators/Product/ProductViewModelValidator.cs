using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Validators.Product;

public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
{
    private readonly ApplicationDbContext _context;

    public ProductViewModelValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name).NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.").MaximumLength(255);
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .MaximumLength(255)
            .Matches("^[a-z0-9-]+$").WithMessage("{PropertyName} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")
            .Must(BeUniqueSlug).WithMessage("{PropertyName} này đã tồn tại. Vui lòng chọn slug khác."); // DB check

        RuleFor(x => x.Description).NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.");
        RuleFor(x => x.ShortDescription).MaximumLength(500);
        RuleFor(x => x.Manufacturer).MaximumLength(255);
        RuleFor(x => x.Origin).MaximumLength(100);

        RuleFor(x => x.ProductTypeId).NotEmpty().WithMessage("Vui lòng chọn {PropertyName}.");
        RuleFor(x => x.Status).IsInEnum().WithMessage("{PropertyName} không hợp lệ.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Vui lòng chọn một {PropertyName}.");

        RuleForEach(x => x.Images).SetValidator(new ProductImageViewModelValidator());
        RuleForEach(x => x.Variants).SetValidator(new ProductVariantViewModelValidator());

    }

    private bool BeUniqueSlug(ProductViewModel viewModel, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return true;
        return !_context.Set<domain.Entities.Product>()
                               .Any(p => p.Slug.ToLower() == slug.ToLower() && p.Id != viewModel.Id);
    }
}