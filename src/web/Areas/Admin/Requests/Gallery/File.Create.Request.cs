using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.StaticFiles;

namespace web.Areas.Admin.Requests.Gallery;
/// <summary>
/// Represents a request to create a new file.      
/// </summary>
public class FileCreateRequest
{
    /// <summary>
    /// Gets or sets the file to upload.    
    /// </summary>
    [Display(Name = "Chọn tệp", Prompt = "Chọn tệp")]
    [Required(ErrorMessage = "Tệp không được bỏ trống")]
    public IFormFile? File { get; set; }

    /// <summary>
    /// Gets or sets the ID of the folder to upload the file to.    
    /// </summary>
    [Display(Name = "Thư mục", Prompt = "Chọn thư mục")]
    public int? FolderId { get; set; }
}

/// <summary>
/// Validator for <see cref="CreateFileRequest"/>.  
/// </summary>
public class FileCreateRequestValidator : AbstractValidator<FileCreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileCreateRequestValidator"/> class.       
    /// </summary>
    public FileCreateRequestValidator()
    {
        RuleFor(request => request.File)
            .NotNull().WithMessage("Tệp không được bỏ trống.");

        RuleFor(x => x.File)
            .Must(file => file == null || file.Length <= 10 * 1024 * 1024)
            .WithMessage("Kích thước tệp không được vượt quá 10MB");

        RuleFor(x => x.File)
            .Must(file => file == null || IsAllowedFileType(file))
            .WithMessage("Loại tệp không được hỗ trợ. Vui lòng chọn tệp hình ảnh.");
    }

    /// <summary>
    /// Determines whether the specified file is an allowed file type using MIME type.
    /// </summary>
    private bool IsAllowedFileType(IFormFile file)
    {
        if (file == null) return false;

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(file.FileName, out var contentType))
        {
            return false; // Nếu không xác định được loại file, từ chối
        }

        var allowedMimeTypes = new[]
        {
            "image/jpeg", "image/png", "image/gif"
        };

        return allowedMimeTypes.Contains(contentType);
    }
}
