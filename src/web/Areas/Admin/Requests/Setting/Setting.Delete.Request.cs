using System.ComponentModel.DataAnnotations;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;

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
    private readonly ApplicationDbContext _dbContext;
    /// <summary>
    /// Initializer a new instance of the <see cref="SettingDeleteRequestValidator"/> class.
    /// </summary>
    public SettingDeleteRequestValidator(ApplicationDbContext context)
    {
        _dbContext = context;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID cài đặt phải là một số nguyên dương.")
            .MustAsync(BeExistingSetting).WithMessage("Cài đặt không tồn tại hoặc đã bị xoá");
    }

    private async Task<bool> BeExistingSetting(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Settings
            .AnyAsync(s => s.Id == id && s.DeletedAt == null, cancellationToken);
    }
}