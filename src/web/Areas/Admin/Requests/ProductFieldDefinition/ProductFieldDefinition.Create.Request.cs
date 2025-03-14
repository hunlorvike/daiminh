using System.ComponentModel.DataAnnotations;
using FluentValidation;
using shared.Enums;

namespace web.Areas.Admin.Requests.ProductFieldDefinition;

/// <summary>
/// Represents a request to create a new product field definition.
/// </summary>
public class ProductFieldDefinitionCreateRequest
{
    /// <summary>
    /// Gets or sets the ID of the product type this field belongs to.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "ID loại sản phẩm là bắt buộc.")] //DataAnnotations for simplicity
    public int ProductTypeId { get; set; }

    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    /// <example>color</example>
    [Display(Name = "Tên trường", Prompt = "Nhập tên của trường")]
    [Required(ErrorMessage = "Tên trường là bắt buộc.")]
    public string? FieldName { get; set; }

    /// <summary>
    /// Gets or sets the data type of the field.
    /// </summary>
    /// <example>Text</example>
    [Display(Name = "Kiểu trường", Prompt = "Chọn kiểu của trường")]
    [Required(ErrorMessage = "Loại trường là bắt buộc.")]  //DataAnnotations for simplicity
    public FieldType? FieldType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field is required.
    /// </summary>
    /// <example>false</example>
    [Display(Name = "Bắt buộc", Prompt = "Trường này có bắt buộc không")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets optional settings for the field (e.g., a JSON string with configuration).
    /// </summary>
    /// <example>{ "allowedValues": ["Red", "Green", "Blue"] }</example>
    [Display(Name = "Tùy chọn trường", Prompt = "Nhập các tùy chọn cho trường (nếu có)")]
    public string? FieldOptions { get; set; }
}

/// <summary>
/// Validator for <see cref="ProductFieldDefinitionCreateRequest"/>.
/// </summary>
public class ProductFieldDefinitionCreateRequestValidator : AbstractValidator<ProductFieldDefinitionCreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductFieldDefinitionCreateRequestValidator"/> class.
    /// </summary>
    public ProductFieldDefinitionCreateRequestValidator()
    {
        RuleFor(request => request.ProductTypeId)
            .GreaterThan(0).WithMessage("ID loại sản phẩm phải là một số nguyên dương.");

        RuleFor(request => request.FieldName)
            .NotEmpty().WithMessage("Tên trường không được bỏ trống.")
            .MaximumLength(50).WithMessage("Tên trường không được vượt quá 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Tên trường chỉ được chứa chữ cái, số và dấu gạch dưới (_).");

        RuleFor(request => request.FieldType)
            .NotNull().WithMessage("Vui lòng chọn kiểu trường.")  // Clearer message
            .IsInEnum().WithMessage("Kiểu trường không hợp lệ. Vui lòng chọn một kiểu trường từ danh sách."); // Combined check
    }
}