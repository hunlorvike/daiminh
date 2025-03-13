using FluentValidation;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.ContentFieldDefinition;

/// <summary>
/// Represents a request to update an existing content field definition.
/// </summary>
public class ContentFieldDefinitionUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the field definition to update.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the content type this field belongs to.
    /// </summary>
    /// <example>2</example>
    [Required(ErrorMessage = "ID loại nội dung là bắt buộc.")]
    public int ContentTypeId { get; set; }

    /// <summary>
    /// Gets or sets the updated name of the field.
    /// </summary>
    /// <example>updated_field_name</example>
    [Display(Name = "Tên trường", Prompt = "Nhập tên của trường")]
    [Required(ErrorMessage = "Tên trường là bắt buộc.")]
    public string? FieldName { get; set; }

    /// <summary>
    /// Gets or sets the updated data type of the field.
    /// </summary>
    /// <example>Number</example>
    [Display(Name = "Kiểu trường", Prompt = "Chọn kiểu của trường")]
    [Required(ErrorMessage = "Loại trường là bắt buộc.")]
    public FieldType? FieldType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field is required.
    /// </summary>
    /// <example>false</example>
    [Display(Name = "Bắt buộc", Prompt = "Trường này có bắt buộc không")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets updated optional settings for the field.
    /// </summary>
    /// <example>{ "minLength": 5 }</example>
    [Display(Name = "Tùy chọn trường", Prompt = "Nhập các tùy chọn cho trường (nếu có)")]
    public string? FieldOptions { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentFieldDefinitionUpdateRequest"/>.
/// </summary>
public class ContentFieldDefinitionUpdateRequestValidator : AbstractValidator<ContentFieldDefinitionUpdateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentFieldDefinitionUpdateRequestValidator"/> class.
    /// </summary>
    public ContentFieldDefinitionUpdateRequestValidator()
    {
        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID trường phải là một số nguyên dương."); // More precise

        RuleFor(request => request.ContentTypeId)
            .GreaterThan(0).WithMessage("ID loại nội dung phải là một số nguyên dương."); // More precise

        RuleFor(request => request.FieldName)
            .NotEmpty().WithMessage("Tên trường không được bỏ trống.")
            .MaximumLength(50).WithMessage("Tên trường không được vượt quá 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Tên trường chỉ được chứa chữ cái, số và dấu gạch dưới (_).");

        RuleFor(request => request.FieldType)
            .NotNull().WithMessage("Vui lòng chọn kiểu trường.") // Clearer message
            .IsInEnum().WithMessage("Kiểu trường không hợp lệ. Vui lòng chọn một kiểu trường từ danh sách."); // Combined checks
    }
}