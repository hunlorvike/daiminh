using FluentValidation;
using web.Areas.Admin.ViewModels.Media;

namespace web.Areas.Admin.Validators;

public class MediaFileViewModelValidator : AbstractValidator<MediaFileViewModel>
{
    public MediaFileViewModelValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("Tên file không được để trống")
            .MaximumLength(100).WithMessage("Tên file không được vượt quá 100 ký tự");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự");

        RuleFor(x => x.AltText)
            .MaximumLength(255).WithMessage("Alt text không được vượt quá 255 ký tự");

        RuleFor(x => x.FileUpload)
            .Must(x => x == null || IsValidFile(x))
            .WithMessage("File không hợp lệ hoặc vượt quá kích thước cho phép (tối đa 20MB)");
    }

    private bool IsValidFile(Microsoft.AspNetCore.Http.IFormFile file)
    {
        if (file == null)
            return true;

        // Check file size (max 20MB)
        if (file.Length > 20 * 1024 * 1024)
            return false;

        return true;
    }
}
