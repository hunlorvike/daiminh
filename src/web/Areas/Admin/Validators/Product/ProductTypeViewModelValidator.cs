using FluentValidation;
using web.Areas.Admin.ViewModels.ProductType;

namespace web.Areas.Admin.Validators.Product;

public class ProductTypeViewModelValidator : AbstractValidator<ProductTypeViewModel>
{
    public ProductTypeViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên loại sản phẩm.")
            .MaximumLength(100).WithMessage("Tên loại sản phẩm không được vượt quá 100 ký tự.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug.")
            .MaximumLength(100).WithMessage("Slug không được vượt quá 100 ký tự.")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự.");

        RuleFor(x => x.Icon)
            .MaximumLength(50).WithMessage("Icon không được vượt quá 50 ký tự.")
            .Matches(@"^ti ti-[a-z0-9\-]+$").WithMessage("Định dạng icon không hợp lệ (ví dụ: ti ti-device-laptop)")
            .When(x => !string.IsNullOrEmpty(x.Icon));
    }
}
