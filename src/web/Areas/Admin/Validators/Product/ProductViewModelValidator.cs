using FluentValidation;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Validators.Product;

public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
{
    public ProductViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên sản phẩm")
            .MaximumLength(255).WithMessage("Tên sản phẩm không được vượt quá 255 ký tự");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Vui lòng nhập mô tả sản phẩm");

        RuleFor(x => x.ShortDescription)
            .MaximumLength(500).WithMessage("Mô tả ngắn không được vượt quá 500 ký tự");

        RuleFor(x => x.Manufacturer)
            .MaximumLength(255).WithMessage("Tên nhà sản xuất không được vượt quá 255 ký tự");

        RuleFor(x => x.Origin)
            .MaximumLength(100).WithMessage("Xuất xứ không được vượt quá 100 ký tự");

        RuleFor(x => x.ProductTypeId)
            .NotEqual(0).WithMessage("Vui lòng chọn loại sản phẩm");

        RuleFor(x => x.MetaTitle)
            .MaximumLength(255).WithMessage("Meta title không được vượt quá 255 ký tự");

        RuleFor(x => x.MetaDescription)
            .MaximumLength(500).WithMessage("Meta description không được vượt quá 500 ký tự");

        RuleFor(x => x.MetaKeywords)
            .MaximumLength(255).WithMessage("Meta keywords không được vượt quá 255 ký tự");
    }
}
