using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
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
    [Display(Name = "Tên trường", Prompt = "Nhập tên trường")]
    [Required(ErrorMessage = "Tên trường là bắt buộc.")]
    public string? FieldName { get; set; }

    /// <summary>
    /// Gets or sets the data type of the field.
    /// </summary>
    /// <example>Text</example>
    [Display(Name = "Loại trường", Prompt = "Chọn loại trường")]
    [Required(ErrorMessage = "Loại trường là bắt buộc.")]  //DataAnnotations for simplicity
    public FieldType? FieldType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field is required.
    /// </summary>
    /// <example>false</example>
    [Display(Name = "Bắt buộc", Prompt = "Có / Không")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets optional settings for the field (e.g., a JSON string with configuration).
    /// </summary>
    /// <example>{ "allowedValues": ["Red", "Green", "Blue"] }</example>
    [Display(Name = "Tùy chọn", Prompt = "Nhập tùy chọn (JSON)")]
    public string? FieldOptions { get; set; }
}

/// <summary>
/// Validator for <see cref="ProductFieldDefinitionCreateRequest"/>.
/// </summary>
public class ProductFieldDefinitionCreateRequestValidator : AbstractValidator<ProductFieldDefinitionCreateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductFieldDefinitionCreateRequestValidator"/> class.
    /// </summary>
    public ProductFieldDefinitionCreateRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(request => request.ProductTypeId)
            .GreaterThan(0).WithMessage("ID loại sản phẩm phải là một số nguyên dương.")
            .MustAsync(BeExistingProductType).WithMessage("Loại sản phẩm không tồn tại hoặc đã bị xóa.");

        RuleFor(request => request.FieldName)
            .NotEmpty().WithMessage("Tên trường không được bỏ trống.")
            .MaximumLength(50).WithMessage("Tên trường không được vượt quá 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Tên trường chỉ được chứa chữ cái, số và dấu gạch dưới (_).")
            .MustAsync(BeUniqueFieldName).WithMessage("Tên trường đã tồn tại trong loại sản phẩm này. Vui lòng chọn một tên khác.");

        RuleFor(request => request.FieldType)
            .NotNull().WithMessage("Vui lòng chọn kiểu trường.")
            .IsInEnum().WithMessage("Kiểu trường không hợp lệ. Vui lòng chọn một kiểu trường từ danh sách.");

        RuleFor(request => request.FieldOptions)
            .Must(BeValidValueLabelArrayJson).WithMessage("Tùy chọn trường phải là một mảng các object JSON với các thuộc tính 'value' và 'label' (ví dụ: [{\"value\": \"option1\", \"label\": \"Option 1\"}]) nếu được cung cấp.")
            .When(request => !string.IsNullOrEmpty(request.FieldOptions));
    }

    /// <summary>
    /// Checks if the ProductType exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingProductType(int productTypeId, CancellationToken cancellationToken)
    {
        return await _dbContext.ProductTypes
            .AnyAsync(pt => pt.Id == productTypeId && pt.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the FieldName is unique within the ProductType.
    /// </summary>
    private async Task<bool> BeUniqueFieldName(ProductFieldDefinitionCreateRequest request, string fieldName, CancellationToken cancellationToken)
    {
        return !await _dbContext.ProductFieldDefinitions
            .AnyAsync(pfd => pfd.FieldName == fieldName && pfd.ProductTypeId == request.ProductTypeId && pfd.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the FieldOptions is a valid JSON array of objects with "value" and "label" properties.
    /// </summary>
    private bool BeValidValueLabelArrayJson(string? fieldOptions)
    {
        try
        {
            var options = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(fieldOptions!);
            return options != null && options.All(opt =>
                opt.ContainsKey("value") && opt["value"] is string &&
                opt.ContainsKey("label") && opt["label"] is string);
        }
        catch
        {
            return false;
        }
    }
}