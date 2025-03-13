using FluentValidation;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.ProductFieldDefinition;

public class ProductFieldDefinitionCreateRequest
{
    public int ProductTypeId { get; set; }

    [Display(Name = "Tên trường", Prompt = "Nhập tên của trường")]
    public string? FieldName { get; set; }

    [Display(Name = "Kiểu trường", Prompt = "Chọn kiểu của trường")]
    [Required(ErrorMessage = "Loại trường là bắt buộc.")]
    public FieldType? FieldType { get; set; }

    [Display(Name = "Bắt buộc", Prompt = "Trường này có bắt buộc không")]
    public bool IsRequired { get; set; }

    [Display(Name = "Tùy chọn trường", Prompt = "Nhập các tùy chọn cho trường (nếu có)")]
    public string? FieldOptions { get; set; }
}

public class ProductFieldDefinitionCreateRequestValidator : AbstractValidator<ProductFieldDefinitionCreateRequest>
{
    public ProductFieldDefinitionCreateRequestValidator()
    {
        RuleFor(request => request.ProductTypeId)
            .NotEmpty().WithMessage("Loại nội dung không được để trống.");

        RuleFor(request => request.FieldName)
            .NotEmpty().WithMessage("Tên trường không được để trống.")
            .MaximumLength(50).WithMessage("Tên trường không được vượt quá 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("Tên trường chỉ được chứa chữ cái, số và dấu gạch dưới.");

        RuleFor(request => request.FieldType)
            .NotNull().WithMessage("Vui lòng chọn một loại trường.")
            .Must(ft => ft != default(FieldType))
            .WithMessage("Vui lòng chọn một loại trường.")
            .IsInEnum().WithMessage("Kiểu trường không hợp lệ.");
    }
}