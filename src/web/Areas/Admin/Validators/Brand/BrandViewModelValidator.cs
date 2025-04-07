using FluentValidation;
using web.Areas.Admin.ViewModels.Brand;

namespace web.Areas.Admin.Validators.Brand;

public class BrandViewModelValidator : AbstractValidator<BrandViewModel>
{
    public BrandViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên thương hiệu")
            .MaximumLength(255).WithMessage("Tên thương hiệu không được vượt quá 255 ký tự");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug chỉ được chứa chữ thường, số và dấu gạch ngang");

        RuleFor(x => x.Description)
            .MaximumLength(4000).WithMessage("Mô tả không được vượt quá 4000 ký tự");

        RuleFor(x => x.LogoUrl)
            .MaximumLength(2048).WithMessage("Đường dẫn logo không được vượt quá 2048 ký tự");

        RuleFor(x => x.Website)
            .MaximumLength(255).WithMessage("Website không được vượt quá 255 ký tự")
            .Matches(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$")
            .When(x => !string.IsNullOrEmpty(x.Website))
            .WithMessage("Website không hợp lệ");

        RuleFor(x => x.MetaTitle)
            .MaximumLength(255).WithMessage("Meta title không được vượt quá 255 ký tự");

        RuleFor(x => x.MetaDescription)
            .MaximumLength(500).WithMessage("Meta description không được vượt quá 500 ký tự");

        RuleFor(x => x.MetaKeywords)
            .MaximumLength(255).WithMessage("Meta keywords không được vượt quá 255 ký tự");
    }
}