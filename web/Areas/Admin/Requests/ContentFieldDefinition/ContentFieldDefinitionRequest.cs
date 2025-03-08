using System.ComponentModel.DataAnnotations;
using core.Common.Enums;
using FluentValidation;

namespace web.Areas.Admin.Requests.ContentFieldDefinition;

public class ContentFieldDefinitionCreateRequest
{
    public int ContentTypeId { get; set; }

    [Display(Name = "Tên trường", Prompt = "Nhập tên của trường")]
    public string? FieldName { get; set; }

    [Display(Name = "Kiểu trường", Prompt = "Chọn kiểu của trường")]
    public FieldType FieldType { get; set; }

    [Display(Name = "Bắt buộc", Prompt = "Trường này có bắt buộc không")]
    public bool IsRequired { get; set; }

    [Display(Name = "Tùy chọn trường", Prompt = "Nhập các tùy chọn cho trường (nếu có)")]
    public string? FieldOptions { get; set; }
}

public class ContentFieldDefinitionUpdateRequest
{
    public int Id { get; set; }

    public int ContentTypeId { get; set; }

    [Display(Name = "Tên trường", Prompt = "Nhập tên của trường")]
    public string? FieldName { get; set; }

    [Display(Name = "Kiểu trường", Prompt = "Chọn kiểu của trường")]
    public FieldType FieldType { get; set; }

    [Display(Name = "Bắt buộc", Prompt = "Trường này có bắt buộc không")]
    public bool IsRequired { get; set; }

    [Display(Name = "Tùy chọn trường", Prompt = "Nhập các tùy chọn cho trường (nếu có)")]
    public string? FieldOptions { get; set; }
}

public class ContentFieldDefinitionDeleteRequest
{
    public int Id { get; set; }
}

public class ContentFieldDefinitionCreateRequestValidator : AbstractValidator<ContentFieldDefinitionCreateRequest>
{
    public ContentFieldDefinitionCreateRequestValidator()
    {
        RuleFor(request => request.ContentTypeId)
            .NotEmpty().WithMessage("Loại nội dung không được để trống.");

        RuleFor(request => request.FieldName)
            .NotEmpty().WithMessage("Tên trường không được để trống.")
            .MaximumLength(50).WithMessage("Tên trường không được vượt quá 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("Tên trường chỉ được chứa chữ cái, số và dấu gạch dưới.");

        RuleFor(request => request.FieldType)
            .IsInEnum().WithMessage("Kiểu trường không hợp lệ.");
    }
}

public class ContentFieldDefinitionUpdateRequestValidator : AbstractValidator<ContentFieldDefinitionUpdateRequest>
{
    public ContentFieldDefinitionUpdateRequestValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("ID không được để trống.");

        RuleFor(request => request.ContentTypeId)
            .NotEmpty().WithMessage("Loại nội dung không được để trống.");

        RuleFor(request => request.FieldName)
            .NotEmpty().WithMessage("Tên trường không được để trống.")
            .MaximumLength(50).WithMessage("Tên trường không được vượt quá 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("Tên trường chỉ được chứa chữ cái, số và dấu gạch dưới.");

        RuleFor(request => request.FieldType)
            .IsInEnum().WithMessage("Kiểu trường không hợp lệ.");
    }
}