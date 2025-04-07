using FluentValidation;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Validators.Product;

public class ProductImageViewModelValidator : AbstractValidator<ProductImageViewModel>
{
    public ProductImageViewModelValidator()
    {
        // ImageUrl is set during upload, maybe validate length?
        RuleFor(x => x.ImageUrl).NotEmpty().MaximumLength(255);
        RuleFor(x => x.ThumbnailUrl).MaximumLength(255);
        RuleFor(x => x.AltText).MaximumLength(255);
        RuleFor(x => x.Title).MaximumLength(255);
        RuleFor(x => x.OrderIndex).GreaterThanOrEqualTo(0);
    }
}