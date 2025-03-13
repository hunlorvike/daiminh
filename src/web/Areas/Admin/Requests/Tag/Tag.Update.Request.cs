using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Tag;

/// <summary>
/// Represents a request to update an existing tag.
/// </summary>
public class TagUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the tag to update.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the updated name of the tag.
    /// </summary>
    /// <example>Updated Technology</example>
    [Display(Name = "Tên thẻ", Prompt = "Nhập tên của thẻ")]
    [Required(ErrorMessage = "Tên thẻ là bắt buộc.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated slug (URL-friendly name) of the tag.
    /// </summary>
    /// <example>updated-technology</example>
    [Display(Name = "Đường dẫn thẻ", Prompt = "Nhập đường dẫn của thẻ")] // Changed to "Đường dẫn thẻ"
    [Required(ErrorMessage = "Đường dẫn thẻ là bắt buộc.")] // Changed to "Đường dẫn thẻ"
    public string Slug { get; set; } = string.Empty;
}

/// <summary>
/// Validator for <see cref="TagUpdateRequest"/>.
/// </summary>
public class TagUpdateRequestValidator : AbstractValidator<TagUpdateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TagUpdateRequestValidator"/> class.
    /// </summary>
    public TagUpdateRequestValidator()
    {
        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID tag phải là một số nguyên dương.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên thẻ không được bỏ trống.") // Improved message
            .MaximumLength(50).WithMessage("Tên thẻ không được vượt quá 50 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.") // Improved message and terminology
            .MaximumLength(50).WithMessage("Đường dẫn (slug) không được vượt quá 50 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang."); // More descriptive
    }
}