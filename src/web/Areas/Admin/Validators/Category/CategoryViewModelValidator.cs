using FluentValidation;
using web.Areas.Admin.ViewModels.Category;

namespace web.Areas.Admin.Validators.Category;

public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
{
    public CategoryViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên danh mục")
            .MaximumLength(100).WithMessage("Tên danh mục không được vượt quá 100 ký tự");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug")
            .MaximumLength(100).WithMessage("Slug không được vượt quá 100 ký tự")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự");

        RuleFor(x => x.Icon)
            .MaximumLength(50).WithMessage("Icon không được vượt quá 50 ký tự");

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Thứ tự hiển thị phải là số không âm");

        RuleFor(x => x.ParentId)
            .Must((model, parentId) => parentId == null || parentId != model.Id)
            .WithMessage("Danh mục không thể là danh mục cha của chính nó");

        RuleFor(x => x.MetaTitle)
            .MaximumLength(100).WithMessage("Meta title không được vượt quá 100 ký tự");

        RuleFor(x => x.MetaDescription)
            .MaximumLength(200).WithMessage("Meta description không được vượt quá 200 ký tự");

        RuleFor(x => x.MetaKeywords)
            .MaximumLength(200).WithMessage("Meta keywords không được vượt quá 200 ký tự");
    }
}