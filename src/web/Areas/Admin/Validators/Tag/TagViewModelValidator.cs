using FluentValidation;
using web.Areas.Admin.ViewModels.Tag;

namespace web.Areas.Admin.Validators.Tag;

public class TagViewModelValidator : AbstractValidator<TagViewModel>
{
    public TagViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên thẻ")
            .MaximumLength(50).WithMessage("Tên thẻ không được vượt quá 50 ký tự");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug")
            .MaximumLength(50).WithMessage("Slug không được vượt quá 50 ký tự")
            .Matches("^[a-z0-9-]+$").WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự");
    }
}