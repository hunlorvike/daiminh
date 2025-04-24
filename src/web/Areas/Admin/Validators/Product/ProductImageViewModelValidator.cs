using FluentValidation;
using web.Areas.Admin.ViewModels.Product;

namespace web.Areas.Admin.Validators.Product;

public class ProductImageViewModelValidator : AbstractValidator<ProductImageViewModel>
{
    public ProductImageViewModelValidator()
    {
        When(img => !img._Delete, () => {
            RuleFor(x => x.ImageUrl)
               .NotEmpty().WithMessage("URL ảnh không được để trống.")
               .MaximumLength(255).WithMessage("URL ảnh không được vượt quá {MaxLength} ký tự.")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("URL ảnh không hợp lệ.");

            RuleFor(x => x.ThumbnailUrl)
               .MaximumLength(255).WithMessage("URL ảnh thumbnail không được vượt quá {MaxLength} ký tự.")
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("URL ảnh thumbnail không hợp lệ.");

            RuleFor(x => x.AltText)
               .MaximumLength(255).WithMessage("Alt Text không được vượt quá {MaxLength} ký tự.");

            RuleFor(x => x.Title)
               .MaximumLength(255).WithMessage("Tiêu đề ảnh không được vượt quá {MaxLength} ký tự.");

            RuleFor(x => x.OrderIndex)
               .GreaterThanOrEqualTo(0).WithMessage("Thứ tự phải là số không âm.");
        });
    }
}