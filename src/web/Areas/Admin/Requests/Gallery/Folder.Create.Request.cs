using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests.Gallery;

/// <summary>
/// Represents a request to create a new folder.
/// </summary>
public class FolderCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the folder.
    /// </summary>
    [Display(Name = "Tên thư mục", Prompt = "Nhập tên thư mục")]
    [Required(ErrorMessage = "Tên thư mục không được bỏ trống.")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the ID of the parent folder.               
    /// </summary>
    [Display(Name = "Thư mục cha", Prompt = "Chọn thư mục cha")]
    public int? ParentId { get; set; }
}


/// <summary>
/// Validator for <see cref="FolderCreateRequest"/>.
/// </summary>
public class FolderCreateRequestValidator : AbstractValidator<FolderCreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FolderCreateRequestValidator"/> class.
    /// </summary>
    public FolderCreateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên thư mục không được bỏ trống.")
            .MaximumLength(50).WithMessage("Tên thư mục không được vượt quá 50 ký tự.")
            .Matches("^[a-zA-Z0-9\\s\\-_]+$")
            .WithMessage("Tên thư mục chỉ được chứa chữ cái, số, dấu gạch ngang và gạch dưới");
    }
}