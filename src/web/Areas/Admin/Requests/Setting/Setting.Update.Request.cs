using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace web.Areas.Admin.Requests.Setting;

/// <summary>
/// Represents a request to update an existing setting.
/// </summary>
public class SettingUpdateRequest
{
    /// <summary>
    /// Gets or sets the ID of the setting.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }

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
    /// <example>My Updated Website</example>
    [Display(Name = "Giá trị", Prompt = "Nhập giá trị")]
    [Required(ErrorMessage = "Giá trị là bắt buộc.")] //DataAnnotation
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the group the setting belongs to.
    /// </summary>
    /// <example>General</example>
    [Display(Name = "Nhóm", Prompt = "Nhập nhóm")]
    [Required(ErrorMessage = "Nhóm là bắt buộc.")] //DataAnnotation
    public string Group { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the setting.
    /// </summary>
    /// <example>Updated description.</example>
    [Display(Name = "Mô tả", Prompt = "Nhập mô tả")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the display order of the setting within its group.
    /// </summary>
    /// <example>2</example>
    [Display(Name = "Thứ tự", Prompt = "Nhập thứ tự")]
    [Required(ErrorMessage = "Thứ tự là bắt buộc.")]
    public int Order { get; set; }
}

/// <summary>
/// Validator for <see cref="SettingUpdateRequest"/>.
/// </summary>
public class SettingUpdateRequestValidator : AbstractValidator<SettingUpdateRequest>
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingUpdateRequestValidator"/> class.
    /// </summary>
    public SettingUpdateRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(request => request.Id)
            .GreaterThan(0).WithMessage("ID cài đặt phải là một số nguyên dương.")
            .MustAsync(BeExistingSetting).WithMessage("Cài đặt không tồn tại hoặc đã bị xóa.");

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
    /// Checks if the setting exists and is not deleted.
    /// </summary>
    private async Task<bool> BeExistingSetting(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Settings
            .AnyAsync(s => s.Id == id && s.DeletedAt == null, cancellationToken);
    }

    /// <summary>
    /// Checks if the Key is unique (excluding the current setting).
    /// </summary>
    private async Task<bool> BeUniqueKey(SettingUpdateRequest request, string key, CancellationToken cancellationToken)
    {
        return !await _dbContext.Settings
            .AnyAsync(s => s.Key == key && s.Id != request.Id && s.DeletedAt == null, cancellationToken);
    }
}