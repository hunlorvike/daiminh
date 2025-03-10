using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Setting;

public class SettingCreateRequest
{
    [Display(Name = "Key", Prompt = "Nhập tên của key")]
    public string? Key { get; set; }

    [Display(Name = "Giá trị của key", Prompt = "Nhập giá trị của key")]
    public string? Value { get; set; }

    [Display(Name = "Nhóm các Key")] public string Group { get; set; }

    [Display(Name = "Ngày tạo")] public DateTime? CreatedAt { get; set; }

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả")]
    public string Description { get; set; }

    [Display(Name = "Thứ tự hiển thị")] public int Order { get; set; }
}

public class SettingUpdateRequest
{
    public int Id { get; set; }

    [Display(Name = "Key", Prompt = "Nhập tên của key")]
    public string? Key { get; set; }

    [Display(Name = "Giá trị của key", Prompt = "Nhập giá trị của key")]
    public string? Value { get; set; }

    [Display(Name = "Nhóm các Key")] public string Group { get; set; }

    [Display(Name = "Ngày tạo")] public DateTime? CreatedAt { get; set; }

    [Display(Name = "Mô tả", Prompt = "Nhập mô tả")]
    public string Description { get; set; }

    [Display(Name = "Thứ tự hiển thị")] public int Order { get; set; }
}

public class SettingDeleteRequest
{
    public int Id { get; set; }
}

public class SettingCreateRequestValidator : AbstractValidator<SettingCreateRequest>
{
    public SettingCreateRequestValidator()
    {
        RuleFor(request => request.Key)
            .NotEmpty().WithMessage("Key không được để trống.")
            .MaximumLength(255).WithMessage("Key không được vượt quá 255 ký tự.");

        RuleFor(request => request.Value)
            .NotEmpty().WithMessage("Value không được để trống.")
            .MaximumLength(255).WithMessage("Value không được vượt quá 255 ký tự.");

        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Group không được để trống.")
            .MaximumLength(100).WithMessage("Group không được vượt quá 100 ký tự.");

        RuleFor(request => request.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự.");
    }
}

public class SettingUpdateRequestValidator : AbstractValidator<SettingUpdateRequest>
{
    public SettingUpdateRequestValidator()
    {
        RuleFor(request => request.Key)
            .NotEmpty().WithMessage("Key không được để trống.")
            .MaximumLength(255).WithMessage("Key không được vượt quá 255 ký tự.");

        RuleFor(request => request.Value)
            .NotEmpty().WithMessage("Value không được để trống.")
            .MaximumLength(255).WithMessage("Value không được vượt quá 255 ký tự.");

        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Group không được để trống.")
            .MaximumLength(100).WithMessage("Group không được vượt quá 100 ký tự.");


        RuleFor(request => request.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự.");
    }
}