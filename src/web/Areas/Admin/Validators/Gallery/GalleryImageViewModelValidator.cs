// GalleryImageViewModelValidator.cs
using FluentValidation;
using web.Areas.Admin.ViewModels.Gallery;

namespace web.Areas.Admin.Validators.Gallery;

public class GalleryImageViewModelValidator : AbstractValidator<GalleryImageViewModel>
{
    public GalleryImageViewModelValidator()
    {
        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Đường dẫn ảnh không được để trống")
            .MaximumLength(255).WithMessage("Đường dẫn ảnh không được vượt quá 255 ký tự")
            .When(x => x.ImageFile == null);

        RuleFor(x => x.ImageFile)
            .NotNull().WithMessage("Vui lòng chọn file ảnh")
            .When(x => string.IsNullOrEmpty(x.ImageUrl));

        RuleFor(x => x.ThumbnailUrl)
            .MaximumLength(255).WithMessage("Đường dẫn thumbnail không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.ThumbnailUrl));

        RuleFor(x => x.AltText)
            .MaximumLength(255).WithMessage("Alt text không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.AltText));

        RuleFor(x => x.Title)
            .MaximumLength(255).WithMessage("Tiêu đề không được vượt quá 255 ký tự")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}