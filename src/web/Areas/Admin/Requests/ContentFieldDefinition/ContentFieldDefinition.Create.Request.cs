using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using shared.Enums;

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
    [Required(ErrorMessage = "ID loại nội dung là bắt buộc.")]
    public int ContentTypeId { get; set; }

    /// <summary>
    /// Gets or sets the name of the field.
    /// </summary>
    /// <example>short_description</example>
    [Display(Name = "Tên trường", Prompt = "Nhập tên trường")]
    [Required(ErrorMessage = "Tên trường là bắt buộc.")]
    public string? FieldName { get; set; }

    /// <summary>
    /// Gets or sets the data type of the field.
    /// </summary>
    /// <example>Text</example>
    [Display(Name = "Loại trường", Prompt = "Chọn loại trường")]
    [Required(ErrorMessage = "Loại trường là bắt buộc.")]
    public FieldType? FieldType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the field is required.
    /// </summary>
    /// <example>true</example>
    [Display(Name = "Bắt buộc", Prompt = "Có / Không")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets optional settings for the field (e.g., a JSON string with configuration).
    /// </summary>
    /// <example>{ "maxLength": 255, "allowedValues": ["option1","option2"] }</example>
    [Display(Name = "Tùy chọn", Prompt = "Nhập tùy chọn (JSON)")]
    public string? FieldOptions { get; set; }
}

/// <summary>
/// Validator for <see cref="ContentFieldDefinitionCreateRequest"/>.
/// </summary>
public class ContentFieldDefinitionCreateRequestValidator : AbstractValidator<ContentFieldDefinitionCreateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentFieldDefinitionCreateRequestValidator"/> class.
    /// </summary>
    public ContentFieldDefinitionCreateRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(request => request.ContentTypeId)
            .GreaterThan(0).WithMessage("ID loại nội dung phải là một số nguyên dương.")
            .MustAsync(BeExistingContentType).WithMessage("Loại nội dung không tồn tại hoặc đã bị xóa.");

        RuleFor(request => request.FieldName)
            .NotEmpty().WithMessage("Tên trường không được bỏ trống.")
            .MaximumLength(50).WithMessage("Tên trường không được vượt quá 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Tên trường chỉ được chứa chữ cái, số và dấu gạch dưới (_).")
            .MustAsync(BeUniqueFieldName).WithMessage("Tên trường đã tồn tại trong loại nội dung này. Vui lòng chọn một tên khác.");

        RuleFor(request => request.FieldType)
            .NotNull().WithMessage("Vui lòng chọn kiểu trường.")
            .IsInEnum().WithMessage("Kiểu trường không hợp lệ. Vui lòng chọn một kiểu trường từ danh sách.");

        RuleFor(request => request.FieldOptions)
            .Must(BeValidValueLabelArrayJson).WithMessage("Tùy chọn trường phải là một mảng chuỗi JSON hợp lệ.")
            .When(request => !string.IsNullOrEmpty(request.FieldOptions));
    }

    /// <summary>
    /// Checks if the ContentType exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingContentType(int contentTypeId, CancellationToken cancellationToken)
    {
        return await _dbContext.ContentTypes
            .AnyAsync(ct => ct.Id == contentTypeId && ct.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the FieldName is unique within the ContentType.
    /// </summary>
    private async Task<bool> BeUniqueFieldName(ContentFieldDefinitionCreateRequest request, string fieldName, CancellationToken cancellationToken)
    {
        return !await _dbContext.ContentFieldDefinitions
            .AnyAsync(cfd => cfd.FieldName == fieldName && cfd.ContentTypeId == request.ContentTypeId && cfd.DeletedAt == null, cancellationToken);
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