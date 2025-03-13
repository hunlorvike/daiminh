using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Setting;

/// <summary>
/// Represents a request to create a new setting.
/// </summary>
public class SettingCreateRequest
{
    /// <summary>
    /// Gets or sets the key of the setting.
    /// </summary>
    [Display(Name = "Key", Prompt = "Nhập tên key")]
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets the value of the setting.
    /// </summary>
    [Display(Name = "Giá trị", Prompt = "Nhập giá trị")]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the group the setting belongs to.
    /// </summary>
    [Display(Name = "Nhóm", Prompt = "Nhập tên nhóm")]
    public string Group { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the setting.
    /// </summary>
    [Display(Name = "Mô tả", Prompt = "Nhập mô tả")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the display order of the setting.
    /// </summary>
    [Display(Name = "Thứ tự hiển thị", Prompt = "Nhập thứ tự")]
    public int Order { get; set; }
}

/// <summary>
/// Validator for <see cref="SettingCreateRequest"/>.
/// </summary>
public class SettingCreateRequestValidator : AbstractValidator<SettingCreateRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingCreateRequestValidator"/> class.
    /// </summary>
    public SettingCreateRequestValidator()
    {
        RuleFor(request => request.Key)
            .NotEmpty().WithMessage("Vui lòng nhập key.")
            .MaximumLength(50).WithMessage("Key không được vượt quá 50 ký tự.");

        RuleFor(request => request.Value)
            .NotEmpty().WithMessage("Vui lòng nhập giá trị.");

        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Vui lòng nhập nhóm.")
            .MaximumLength(100).WithMessage("Nhóm không được vượt quá 100 ký tự.");

        RuleFor(request => request.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự.");
    }
}