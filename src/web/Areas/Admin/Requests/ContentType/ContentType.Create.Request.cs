using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.ContentType;

/// <summary>
/// Represents a request to create a new content type.
/// </summary>
public class ContentTypeCreateRequest
{
    /// <summary>
    /// Gets or sets the name of the content type.
    /// </summary>
    /// <example>Blog Post</example>
    [Display(Name = "Tên loại bài viết", Prompt = "Nhập tên của loại bài viết")]
    [Required(ErrorMessage = "Tên loại bài viết không được bỏ trống.")] // Use DataAnnotations
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the slug (URL-friendly name) of the content type.
    /// </summary>
    /// <example>blog-post</example>
    [Display(Name = "Đường dẫn bài viết", Prompt = "Nhập đường dẫn của loại bài viết")]
    [Required(ErrorMessage = "Đường dẫn loại bài viết không được bỏ trống.")] // Use DataAnnotations
    public string? Slug { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentTypeCreateRequest"/>.
/// </summary>
public class ContentTypeCreateRequestValidator : AbstractValidator<ContentTypeCreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentTypeCreateRequestValidator"/> class.
    /// </summary>
    public ContentTypeCreateRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên loại nội dung không được bỏ trống.") // Improved message
            .MaximumLength(50).WithMessage("Tên loại nội dung không được vượt quá 50 ký tự."); // Limit to 50, like the entity

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.") // Improved message and terminology
            .MaximumLength(50).WithMessage("Đường dẫn (slug) không được vượt quá 50 ký tự.") // Limit to 50, like the entity
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang."); // More descriptive
    }
}