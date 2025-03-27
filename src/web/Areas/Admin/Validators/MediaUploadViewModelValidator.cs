using FluentValidation;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Validators;

public class MediaUploadViewModelValidator : AbstractValidator<MediaUploadViewModel>
{
    public MediaUploadViewModelValidator()
    {
        RuleFor(x => x.Files)
            .NotEmpty().WithMessage("Vui lòng chọn ít nhất một file để tải lên");

        RuleForEach(x => x.Files)
            .Must(IsValidFile)
            .WithMessage("File không hợp lệ hoặc vượt quá kích thước cho phép (tối đa 20MB)");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự");

        RuleFor(x => x.AltText)
            .MaximumLength(255).WithMessage("Alt text không được vượt quá 255 ký tự");
    }

    private bool IsValidFile(Microsoft.AspNetCore.Http.IFormFile file)
    {
        if (file == null)
            return false;

        // Check file size (max 20MB)
        if (file.Length > 20 * 1024 * 1024)
            return false;

        return true;
    }
}
