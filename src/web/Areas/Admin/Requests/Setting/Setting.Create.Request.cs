using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.Setting;

/// <summary>
/// Represents a request to create a new setting.
/// </summary>
public class SettingCreateRequest
{
    /// <summary>
    /// Gets or sets the key of the setting.  This should be unique.
    /// </summary>
    /// <example>site_name</example>
    [Display(Name = "Khóa", Prompt = "Nhập khóa")]
    [Required(ErrorMessage = "Key là bắt buộc.")] //DataAnnotations
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets the value of the setting.
    /// </summary>
    /// <example>My Awesome Website</example>
    [Display(Name = "Giá trị", Prompt = "Nhập giá trị")]
    [Required(ErrorMessage = "Giá trị là bắt buộc.")] //DataAnnotation
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the group the setting belongs to.  This is used for organization.
    /// </summary>
    /// <example>General</example>
    [Display(Name = "Nhóm", Prompt = "Nhập nhóm")]
    [Required(ErrorMessage = "Nhóm là bắt buộc.")] //DataAnnotation
    public string Group { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the setting.
    /// </summary>
    /// <example>The name of the website, displayed in the title bar.</example>
    [Display(Name = "Mô tả", Prompt = "Nhập mô tả")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the display order of the setting within its group.
    /// </summary>
    /// <example>1</example>
    [Display(Name = "Thứ tự", Prompt = "Nhập thứ tự")]
    [Required(ErrorMessage = "Thứ tự là bắt buộc.")]
    public int Order { get; set; }
}

/// <summary>
/// Validator for <see cref="SettingCreateRequest"/>.
/// </summary>
public class SettingCreateRequestValidator : AbstractValidator<SettingCreateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingCreateRequestValidator"/> class.
    /// </summary>
    public SettingCreateRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(request => request.Key)
            .NotEmpty().WithMessage("Key cài đặt không được bỏ trống.")
            .MaximumLength(50).WithMessage("Key cài đặt không được vượt quá 50 ký tự.")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Key cài đặt chỉ được chứa chữ cái, số và dấu gạch dưới (_).")
            .MustAsync(BeUniqueKey).WithMessage("Key cài đặt đã tồn tại. Vui lòng chọn một key khác.");

        RuleFor(request => request.Value)
            .NotEmpty().WithMessage("Giá trị cài đặt không được bỏ trống.");

        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Nhóm cài đặt không được bỏ trống.")
            .MaximumLength(100).WithMessage("Nhóm cài đặt không được vượt quá 100 ký tự.");

        RuleFor(request => request.Description)
            .MaximumLength(500).WithMessage("Mô tả cài đặt không được vượt quá 500 ký tự.");

        RuleFor(request => request.Order)
            .GreaterThanOrEqualTo(0).WithMessage("Thứ tự hiển thị phải là một số nguyên không âm.");
    }

    /// <summary>
    /// Checks if the Key is unique in the database.
    /// </summary>
    private async Task<bool> BeUniqueKey(string key, CancellationToken cancellationToken)
    {
        return !await _dbContext.Settings
            .AnyAsync(s => s.Key == key && s.DeletedAt == null, cancellationToken);
    }
}