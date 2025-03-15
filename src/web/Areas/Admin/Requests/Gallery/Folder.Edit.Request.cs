using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests.Gallery;

public class FolderEditRequest
{
    /// <summary>
    /// Gets or sets the ID of the folder to rename.
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the new name of the folder.
    /// </summary>
    [Display(Name = "Tên thư mục", Prompt = "Nhập tên mới của thư mục")]
    [Required(ErrorMessage = "Tên thư mục không được bỏ trống.")]
    public string? Name { get; set; }
}

public class FolderEditRequestValidator : AbstractValidator<FolderEditRequest>
{
    public FolderEditRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("ID thư mục không được bỏ trống.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên thư mục không được bỏ trống.")
            .MaximumLength(100).WithMessage("Tên thư mục không được vượt quá 100 ký tự")
            .Matches("^[a-zA-Z0-9\\s\\-_]+$")
            .WithMessage("Tên thư mục chỉ được chứa chữ cái, số, dấu gạch ngang và gạch dưới");
    }
}