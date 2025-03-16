using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace web.Areas.Admin.Requests.ContentType;

/// <summary>
/// Represents a request to update an existing content type.
/// </summary>
public class ContentTypeUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the content type to update.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the updated name of the content type.
    /// </summary>
    /// <example>News Article</example>
    [Display(Name = "Tên loại nội dung", Prompt = "Nhập tên loại nội dung")]
    [Required(ErrorMessage = "Tên loại nội dung không được bỏ trống.")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the updated slug (URL-friendly name) of the content type.
    /// </summary>
    /// <example>news-article</example>
    [Display(Name = "Đường dẫn", Prompt = "Nhập đường dẫn")]
    [Required(ErrorMessage = "Đường dẫn loại nội dung không được bỏ trống.")]
    public string? Slug { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentTypeUpdateRequest"/>.
/// </summary>
public class ContentTypeUpdateRequestValidator : AbstractValidator<ContentTypeUpdateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentTypeUpdateRequestValidator"/> class.
    /// </summary>
    public ContentTypeUpdateRequestValidator()
    {
        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID loại nội dung phải là một số nguyên dương.");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Tên loại nội dung không được bỏ trống.")  // Improved message
            .MaximumLength(50).WithMessage("Tên loại nội dung không được vượt quá 50 ký tự."); // Limit to 50, like the entity

        RuleFor(request => request.Slug)
            .NotEmpty().WithMessage("Đường dẫn (slug) không được bỏ trống.") // Improved message and terminology
            .MaximumLength(50).WithMessage("Đường dẫn (slug) không được vượt quá 50 ký tự.") // Limit to 50, like the entity
            .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Đường dẫn (slug) chỉ được chứa chữ cái thường, số và dấu gạch ngang (-), và không được bắt đầu hoặc kết thúc bằng dấu gạch ngang.");  // More descriptive
    }
}
