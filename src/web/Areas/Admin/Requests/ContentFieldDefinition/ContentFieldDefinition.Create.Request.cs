using FluentValidation;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.ContentFieldDefinition;

/// <summary>
/// Represents a request to create a new content field definition.
/// </summary>
public class ContentFieldDefinitionCreateRequest
{
    /// <summary>
    /// Gets or sets the ID of the content type this field belongs to.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "ID loại nội dung là bắt buộc.")] // Use DataAnnotations for simple checks
    public int ContentTypeId { get; set; }

    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    /// <example>short_description</example>
    [Display(Name = "Tên trường", Prompt = "Nhập tên của trường")]
    [Required(ErrorMessage = "Tên trường là bắt buộc.")]
    public string? FieldName { get; set; }

    /// <summary>
    /// Gets or sets the data type of the field.
    /// </summary>
    /// <example>Text</example>
    [Display(Name = "Kiểu trường", Prompt = "Chọn kiểu của trường")]
    [Required(ErrorMessage = "Loại trường là bắt buộc.")]
    public FieldType? FieldType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field is required.
    /// </summary>
    /// <example>true</example>
    [Display(Name = "Bắt buộc", Prompt = "Trường này có bắt buộc không")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets optional settings for the field (e.g., a JSON string with configuration).
    /// </summary>
    /// <example>{ "maxLength": 255, "allowedValues": ["option1", "option2"] }</example>
    [Display(Name = "Tùy chọn trường", Prompt = "Nhập các tùy chọn cho trường (nếu có)")]
    public string? FieldOptions { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentFieldDefinitionCreateRequest"/>.
/// </summary>
public class ContentFieldDefinitionCreateRequestValidator : AbstractValidator<ContentFieldDefinitionCreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentFieldDefinitionCreateRequestValidator"/> class.
    /// </summary>
    public ContentFieldDefinitionCreateRequestValidator()
    {
        RuleFor(request => request.ContentTypeId)
            .GreaterThan(0).WithMessage("ID loại nội dung phải là một số nguyên dương."); // More precise validation

        RuleFor(request => request.FieldName)
            .NotEmpty().WithMessage("Tên trường không được bỏ trống.")
            .MaximumLength(50).WithMessage("Tên trường không được vượt quá 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Tên trường chỉ được chứa chữ cái, số và dấu gạch dưới (_).");

        RuleFor(request => request.FieldType)
            .NotNull().WithMessage("Vui lòng chọn kiểu trường.") // Clearer message
            .IsInEnum().WithMessage("Kiểu trường không hợp lệ. Vui lòng chọn một kiểu trường từ danh sách."); // Combined checks
    }
}