// GalleryViewModelValidator.cs
using FluentValidation;
using System.Text.RegularExpressions;
using web.Areas.Admin.ViewModels.Gallery;

namespace web.Areas.Admin.Validators.Gallery;

public class GalleryViewModelValidator : AbstractValidator<GalleryViewModel>
{
    public GalleryViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vui lòng nhập tên thư viện")
            .MaximumLength(255).WithMessage("Tên thư viện không được vượt quá 255 ký tự");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Vui lòng nhập slug")
            .MaximumLength(255).WithMessage("Slug không được vượt quá 255 ký tự")
            .Matches(new Regex("^[a-z0-9]+(?:-[a-z0-9]+)*$"))
            .WithMessage("Slug chỉ được chứa chữ cái thường, số và dấu gạch ngang");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.CoverImage)
            .MaximumLength(255).WithMessage("Đường dẫn ảnh bìa không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.CoverImage));

        RuleFor(x => x.MetaTitle)
            .MaximumLength(255).WithMessage("Meta title không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.MetaTitle));

        RuleFor(x => x.MetaDescription)
            .MaximumLength(500).WithMessage("Meta description không được vượt quá 500 ký tự")
            .When(x => !string.IsNullOrEmpty(x.MetaDescription));

        RuleFor(x => x.MetaKeywords)
            .MaximumLength(255).WithMessage("Meta keywords không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.MetaKeywords));

        RuleFor(x => x.CategoryIds)
            .Must(x => x.Count > 0)
            .WithMessage("Vui lòng chọn ít nhất một danh mục");
    }
}