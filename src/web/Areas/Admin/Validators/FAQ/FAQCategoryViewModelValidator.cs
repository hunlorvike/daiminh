using FluentValidation;
using web.Areas.Admin.ViewModels.FAQ;

namespace web.Areas.Admin.Validators.FAQ;

public class FAQCategoryViewModelValidator : AbstractValidator<FAQCategoryViewModel>
{
    public FAQCategoryViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên danh mục")
            .MaximumLength(100).WithMessage("Tên danh mục không được vượt quá 100 ký tự");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug")
            .MaximumLength(100).WithMessage("Slug không được vượt quá 100 ký tự")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Thứ tự hiển thị phải là số không âm");
    }
}