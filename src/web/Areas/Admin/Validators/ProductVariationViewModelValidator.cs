using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels;

namespace web.Areas.Admin.Validators;

public class ProductVariationViewModelValidator : AbstractValidator<ProductVariationViewModel>
{
    private readonly ApplicationDbContext _context;

    public ProductVariationViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Biến thể phải thuộc về một Sản phẩm.")
            .Must(ProductExists).WithMessage("Sản phẩm cha không tồn tại.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải là số không âm.");

        RuleFor(x => x.SalePrice)
            .GreaterThanOrEqualTo(0).When(x => x.SalePrice.HasValue).WithMessage("{PropertyName} phải là số không âm.")
            .LessThanOrEqualTo(x => x.Price).When(x => x.SalePrice.HasValue && x.Price > 0).WithMessage("{PropertyName} phải nhỏ hơn hoặc bằng Giá bán.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải là số không âm.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.ImageUrl)).WithMessage("URL Ảnh không được vượt quá {MaxLength} ký tự.");

        RuleFor(x => x.SelectedAttributeValueIds)
            .NotNull().WithMessage("Vui lòng chọn thuộc tính cho biến thể.")
            .Must(BeValidAttributeValuesForProduct).WithMessage("Các thuộc tính được chọn không hợp lệ cho sản phẩm này.");

        RuleFor(x => x)
            .Must(BeUniqueAttributeValueCombinationForProduct).WithMessage("Biến thể với sự kết hợp thuộc tính này đã tồn tại cho sản phẩm này.");

        When(x => x.IsDefault, () =>
        {
            RuleFor(x => x.IsDefault)
                 .Must(BeOnlyDefaultVariation).WithMessage("Chỉ có thể có một biến thể mặc định cho mỗi sản phẩm.");
        });
    }

    private bool ProductExists(int productId)
    {
        return _context.Products.Any(p => p.Id == productId);
    }

    private bool BeValidAttributeValuesForProduct(ProductVariationViewModel viewModel, List<int>? selectedAttributeValueIds)
    {
        if (selectedAttributeValueIds == null || !selectedAttributeValueIds.Any())
        {
            var requiredAttributeCountForProduct = _context.ProductAttributes
                .Count(pa => pa.ProductId == viewModel.ProductId);
            if (requiredAttributeCountForProduct > 0 && (selectedAttributeValueIds == null || !selectedAttributeValueIds.Any()))
            {
                return false;
            }
            return true;
        }

        var productAttributeIds = _context.ProductAttributes
            .Where(pa => pa.ProductId == viewModel.ProductId)
            .Select(pa => pa.AttributeId)
            .ToList();

        if (!productAttributeIds.Any())
        {
            return !selectedAttributeValueIds.Any();
        }

        var validAttributeValueIds = _context.AttributeValues
            .Where(av => selectedAttributeValueIds.Contains(av.Id) && productAttributeIds.Contains(av.AttributeId))
            .Select(av => av.Id)
            .ToList();

        return selectedAttributeValueIds.Count == validAttributeValueIds.Count;
    }

    private bool BeUniqueAttributeValueCombinationForProduct(ProductVariationViewModel viewModel)
    {
        if (viewModel.ProductId <= 0 || viewModel.SelectedAttributeValueIds == null || !viewModel.SelectedAttributeValueIds.Any())
        {
            return true;
        }

        var normalizedSelectedIds = viewModel.SelectedAttributeValueIds.OrderBy(id => id).ToList();

        var existingVariations = _context.ProductVariations
            .Where(v => v.ProductId == viewModel.ProductId && v.Id != viewModel.Id)
            .Include(v => v.ProductVariationAttributeValues)
            .ToList();

        foreach (var existingVariation in existingVariations)
        {
            var existingIds = existingVariation.ProductVariationAttributeValues?
                                             .Select(pvav => pvav.AttributeValueId)
                                             .OrderBy(id => id)
                                             .ToList() ?? new List<int>();

            if (existingIds.SequenceEqual(normalizedSelectedIds))
            {
                return false;
            }
        }

        return true;
    }

    private bool BeOnlyDefaultVariation(ProductVariationViewModel viewModel, bool isDefault)
    {
        if (!isDefault)
        {
            return true;
        }

        var otherDefaultVariationsCount = _context.ProductVariations
            .Count(v => v.ProductId == viewModel.ProductId && v.Id != viewModel.Id && v.IsDefault);

        return otherDefaultVariationsCount == 0;
    }
}
