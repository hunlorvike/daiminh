using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests.Gallery;

/// <summary>
/// Represents a request to edit a file.
/// </summary>
public class FileEditRequest
{
    /// <summary>
    /// Gets or sets the ID of the file to edit.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the file.  
    /// </summary>
    [Display(Name = "Tên tệp", Prompt = "Nhập tên mới của tệp")]
    [Required(ErrorMessage = "Tên tệp không được bỏ trống.")]
    public string? Name { get; set; }
}

/// <summary>
/// Validator for <see cref="FileEditRequest"/>.
/// </summary>
public class FileEditRequestValidator : AbstractValidator<FileEditRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileEditRequestValidator"/> class. 
    /// </summary>
    public FileEditRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("ID thư mục không được bỏ trống.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên tệp không được bỏ trống.")
            .MaximumLength(100).WithMessage("Tên tệp không được vượt quá 100 ký tự")
            .Must(name => !string.IsNullOrWhiteSpace(name) &&
                  !name.Contains("/") &&
                  !name.Contains("\\") &&
                  !name.Contains(":") &&
                  !name.Contains("*") &&
                  !name.Contains("?") &&
                  !name.Contains("\"") &&
                  !name.Contains("<") &&
                  !name.Contains(">") &&
                  !name.Contains("|"))
            .WithMessage("Tên tệp không được chứa các ký tự đặc biệt không hợp lệ (/ \\ : * ? \" < > |)");
    }
}
