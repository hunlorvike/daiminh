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

        RuleFor(x => x.SelectedAttributeValueIds)
            .NotEmpty().WithMessage("Vui lòng chọn các giá trị thuộc tính cho biến thể.")
            .MustAsync(BeValidAttributeValuesAsync).WithMessage("Một hoặc nhiều giá trị thuộc tính được chọn không hợp lệ.")
            .MustAsync(BelongToProductAttributesAsync).WithMessage("Một hoặc nhiều giá trị thuộc tính được chọn không thuộc các thuộc tính của sản phẩm này.")
            .MustAsync(HaveOneValuePerProductAttributeAsync).WithMessage("Phải chọn chính xác một giá trị cho mỗi thuộc tính của sản phẩm này.");
    }

    private async Task<bool> BeValidAttributeValuesAsync(List<int>? valueIds, CancellationToken cancellationToken)
    {
        if (valueIds == null || !valueIds.Any()) return true;

        var distinctValueIds = valueIds.Distinct().ToList();
        var existingCount = await _context.Set<domain.Entities.AttributeValue>()
                                          .Where(av => distinctValueIds.Contains(av.Id))
                                          .CountAsync(cancellationToken);

        return existingCount == distinctValueIds.Count;
    }

    private async Task<bool> BelongToProductAttributesAsync(ProductVariationViewModel viewModel, List<int>? valueIds, CancellationToken cancellationToken)
    {
        if (valueIds == null || !valueIds.Any()) return true;

        var productAttributeIds = await _context.Set<ProductAttribute>()
                                                .Where(pa => pa.ProductId == viewModel.ProductId)
                                                .Select(pa => pa.AttributeId)
                                                .ToListAsync(cancellationToken);

        if (!productAttributeIds.Any())
        {
            return !valueIds.Any();
        }

        var selectedAttributeValueAttributeIds = await _context.Set<domain.Entities.AttributeValue>()
                                                               .Where(av => valueIds.Contains(av.Id))
                                                               .Select(av => av.AttributeId)
                                                               .Distinct()
                                                               .ToListAsync(cancellationToken);

        return selectedAttributeValueAttributeIds.All(attrId => productAttributeIds.Contains(attrId));
    }

    private async Task<bool> HaveOneValuePerProductAttributeAsync(ProductVariationViewModel viewModel, List<int>? valueIds, CancellationToken cancellationToken)
    {
        if (valueIds == null) return false;

        var productAttributeIds = await _context.Set<ProductAttribute>()
                                               .Where(pa => pa.ProductId == viewModel.ProductId)
                                               .Select(pa => pa.AttributeId)
                                               .ToListAsync(cancellationToken);

        if (!productAttributeIds.Any())
        {
            return !valueIds.Any();
        }

        var selectedAttributeIdsForValues = await _context.Set<domain.Entities.AttributeValue>()
                                                          .Where(av => valueIds.Contains(av.Id))
                                                          .Select(av => av.AttributeId)
                                                          .Distinct()
                                                          .ToListAsync(cancellationToken);

        if (selectedAttributeIdsForValues.Count != productAttributeIds.Count)
        {
            return false;
        }

        foreach (var productAttrId in productAttributeIds)
        {
            var count = await _context.Set<domain.Entities.AttributeValue>()
                                      .Where(av => valueIds.Contains(av.Id) && av.AttributeId == productAttrId)
                                      .CountAsync(cancellationToken);
            if (count != 1)
            {
                return false; 
            }
        }

        return true; 
    }


    private bool BeUniqueCombination(ProductVariationViewModel viewModel, List<int>? valueIds, CancellationToken cancellationToken)
    {

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

        return true; 
    }
}