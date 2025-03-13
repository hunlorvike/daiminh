using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Setting;

/// <summary>
/// Represents a request to delete a setting.
/// </summary>
public class SettingDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the setting to delete.
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "Id không được bỏ trống")]
    public int Id { get; set; }
}

/// <summary>
/// Validator for <see cref="SettingDeleteRequest"/>
/// </summary>
public class SettingDeleteRequestValidator : AbstractValidator<SettingDeleteRequest>
{
    /// <summary>
    /// Initializer a new instance of the <see cref="SettingDeleteRequestValidator"/> class.
    /// </summary>
    public SettingDeleteRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID cài đặt phải là một số nguyên dương.");
    }
}