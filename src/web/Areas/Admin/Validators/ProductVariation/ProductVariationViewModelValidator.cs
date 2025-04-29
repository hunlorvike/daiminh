using domain.Entities;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using web.Areas.Admin.ViewModels.ProductVariation;

namespace web.Areas.Admin.Validators.ProductVariation;

public class ProductVariationViewModelValidator : AbstractValidator<ProductVariationViewModel>
{
    private readonly ApplicationDbContext _context;

    public ProductVariationViewModelValidator(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Sản phẩm gốc không được để trống.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Vui lòng nhập {PropertyName}.")
            .GreaterThan(0).WithMessage("{PropertyName} phải lớn hơn 0.");

        RuleFor(x => x.SalePrice)
             .GreaterThan(0).When(x => x.SalePrice.HasValue).WithMessage("{PropertyName} phải lớn hơn 0.")
             .LessThanOrEqualTo(x => x.Price).When(x => x.SalePrice.HasValue).WithMessage("{PropertyName} phải nhỏ hơn hoặc bằng Giá bán.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} phải là số không âm.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.ImageUrl));

        // --- Validation for SelectedAttributeValueIds ---
        // This is the complex part. We need to ensure:
        // 1. All selected AttributeValueIds are valid and belong to actual AttributeValues.
        // 2. Each selected AttributeValue belongs to an Attribute that is associated with the Parent Product (via ProductAttribute).
        // 3. There is exactly ONE selected AttributeValue for EACH Attribute associated with the Parent Product.
        // 4. The combination of selected AttributeValues is unique for this Product.

        RuleFor(x => x.SelectedAttributeValueIds)
            .NotEmpty().WithMessage("Vui lòng chọn các giá trị thuộc tính cho biến thể.")
            .MustAsync(BeValidAttributeValuesAsync).WithMessage("Một hoặc nhiều giá trị thuộc tính được chọn không hợp lệ.")
            .MustAsync(BelongToProductAttributesAsync).WithMessage("Một hoặc nhiều giá trị thuộc tính được chọn không thuộc các thuộc tính của sản phẩm này.")
            .MustAsync(HaveOneValuePerProductAttributeAsync).WithMessage("Phải chọn chính xác một giá trị cho mỗi thuộc tính của sản phẩm này.");
        //.MustAsync(BeUniqueCombination).WithMessage("Một biến thể với sự kết hợp các thuộc tính này đã tồn tại.");

    }

    // Helper to check if all selected AttributeValueIds are valid IDs
    private async Task<bool> BeValidAttributeValuesAsync(List<int>? valueIds, CancellationToken cancellationToken)
    {
        if (valueIds == null || !valueIds.Any()) return true;

        var distinctValueIds = valueIds.Distinct().ToList();
        var existingCount = await _context.Set<domain.Entities.AttributeValue>()
                                          .Where(av => distinctValueIds.Contains(av.Id))
                                          .CountAsync(cancellationToken);

        return existingCount == distinctValueIds.Count;
    }

    // Helper to check if selected AttributeValueIds belong to Attributes linked to the Product
    private async Task<bool> BelongToProductAttributesAsync(ProductVariationViewModel viewModel, List<int>? valueIds, CancellationToken cancellationToken)
    {
        if (valueIds == null || !valueIds.Any()) return true;

        // Get the IDs of Attributes linked to the parent product
        var productAttributeIds = await _context.Set<ProductAttribute>()
                                                .Where(pa => pa.ProductId == viewModel.ProductId)
                                                .Select(pa => pa.AttributeId)
                                                .ToListAsync(cancellationToken);

        if (!productAttributeIds.Any())
        {
            // If the product has no attributes configured, no values should be selected
            return !valueIds.Any();
        }

        // Get the AttributeIds for the selected AttributeValueIds
        var selectedAttributeValueAttributeIds = await _context.Set<domain.Entities.AttributeValue>()
                                                               .Where(av => valueIds.Contains(av.Id))
                                                               .Select(av => av.AttributeId)
                                                               .Distinct()
                                                               .ToListAsync(cancellationToken);

        // Check if all AttributeIds of selected values are present in the product's attribute IDs
        return selectedAttributeValueAttributeIds.All(attrId => productAttributeIds.Contains(attrId));
    }

    // Helper to check if there is exactly one value selected per product attribute
    private async Task<bool> HaveOneValuePerProductAttributeAsync(ProductVariationViewModel viewModel, List<int>? valueIds, CancellationToken cancellationToken)
    {
        if (valueIds == null) return false; // Handled by NotEmpty

        // Get the IDs of Attributes linked to the parent product
        var productAttributeIds = await _context.Set<ProductAttribute>()
                                               .Where(pa => pa.ProductId == viewModel.ProductId)
                                               .Select(pa => pa.AttributeId)
                                               .ToListAsync(cancellationToken);

        // If the product has no attributes, there should be no selected values
        if (!productAttributeIds.Any())
        {
            return !valueIds.Any();
        }

        // Get the AttributeIds for the selected AttributeValueIds
        var selectedAttributeIdsForValues = await _context.Set<domain.Entities.AttributeValue>()
                                                          .Where(av => valueIds.Contains(av.Id))
                                                          .Select(av => av.AttributeId)
                                                          .Distinct()
                                                          .ToListAsync(cancellationToken);

        // Ensure the number of distinct selected AttributeIds equals the number of product attributes
        if (selectedAttributeIdsForValues.Count != productAttributeIds.Count)
        {
            return false;
        }

        // Ensure each AttributeId of the product has exactly one value selected
        foreach (var productAttrId in productAttributeIds)
        {
            var count = await _context.Set<domain.Entities.AttributeValue>()
                                      .Where(av => valueIds.Contains(av.Id) && av.AttributeId == productAttrId)
                                      .CountAsync(cancellationToken);
            if (count != 1)
            {
                return false; // Found an attribute with more or less than one value selected
            }
        }

        return true; // Passed all checks
    }


    // Helper to check if the combination of selected AttributeValues is unique for this Product
    private bool BeUniqueCombination(ProductVariationViewModel viewModel, List<int>? valueIds, CancellationToken cancellationToken)
    {
        //if (valueIds == null || !valueIds.Any()) return true; // Handled by NotEmpty/HaveOneValuePerProductAttribute

        //// Sort the IDs to compare combinations regardless of order
        //var sortedSelectedValueIds = valueIds.OrderBy(id => id).ToList();

        //// Find variations for this product, excluding the one being edited (if any)
        //var query = _context.Set<domain.Entities.AttributeValue>()
        //                    .Where(pv => pv.ProductId == viewModel.ProductId && pv.Id != viewModel.Id)
        //                    .Include(pv => pv.ProductVariationAttributeValues)
        //                    .AsNoTracking();

        //var existingVariations = await query.ToListAsync(cancellationToken);

        //foreach (var existingVariation in existingVariations)
        //{
        //    var existingValueIds = existingVariation.ProductVariationAttributeValues?
        //                                            .Select(pvav => pvav.AttributeValueId)
        //                                            .OrderBy(id => id)
        //                                            .ToList();

        //    // Compare sorted lists
        //    if (existingValueIds != null && existingValueIds.SequenceEqual(sortedSelectedValueIds))
        //    {
        //        return false; // Found a matching combination
        //    }
        //}

        return true; // Combination is unique
    }
}