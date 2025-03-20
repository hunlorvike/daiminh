
using FluentValidation;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Tag;

/// <summary>
/// Represents a request to create a new tag.
/// </summary>
public class TagCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    /// <example>Technology</example>
    [Display(Name = "Tên thẻ", Prompt = "Nhập tên thẻ")]
    [Required(ErrorMessage = "Tên thẻ là bắt buộc.")] //DataAnnotations
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the slug (URL-friendly name) of the tag.
    /// </summary>
    /// <example>technology</example>
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]  // Changed to "Đường dẫn thẻ"
    [Required(ErrorMessage = "Đường dẫn thẻ là bắt buộc.")] //DataAnnotations, and changed to "Đường dẫn thẻ"
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the entity type of the tag.
    /// </summary>
    [Display(Name = "Loại thẻ", Prompt = "Chọn loại thẻ")]
    public EntityType EntityType { get; set; } = EntityType.Product;
}

/// <summary>
/// Validator for <see cref="TagCreateRequest"/>.
/// </summary>
public class TagCreateRequestValidator : AbstractValidator<TagCreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TagCreateRequestValidator"/> class.
    /// </summary>
    public TagCreateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên thẻ không được bỏ trống.") // Improved message
            .MaximumLength(50).WithMessage("Tên thẻ không được vượt quá 50 ký tự.");

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.") // Improved message and terminology
            .MaximumLength(50).WithMessage("Đường dẫn (slug) không được vượt quá 50 ký tự.")
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang."); // More descriptive

        RuleFor(request => request.EntityType)
            .IsInEnum().WithMessage("Loại thẻ không hợp lệ."); // More descriptive
    }
}