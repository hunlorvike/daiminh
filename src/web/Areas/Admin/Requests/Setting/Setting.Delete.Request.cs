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
    public int Id { get; set; }
}