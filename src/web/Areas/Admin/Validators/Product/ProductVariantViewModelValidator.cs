using FluentValidation;
using web.Areas.Admin.ViewModels.Product;
namespace web.Areas.Admin.Validators.Product;
public class ProductVariantViewModelValidator : AbstractValidator<ProductVariantViewModel>
{
    public ProductVariantViewModelValidator()
    {
        When(x => !x.IsDeleted, () =>
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên biến thể không được rỗng.").MaximumLength(255);
            RuleFor(x => x.Sku).NotEmpty().WithMessage("SKU không được rỗng.").MaximumLength(100);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Giá phải lớn hơn hoặc bằng 0.");
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0).WithMessage("Số lượng tồn kho phải lớn hơn hoặc bằng 0.");
            RuleFor(x => x.Color).MaximumLength(50);
            RuleFor(x => x.Size).MaximumLength(50);
            RuleFor(x => x.Packaging).MaximumLength(50);
            RuleFor(x => x.ImageUrl).MaximumLength(2048);
        });
    }
}