namespace shared.Enums;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a generic status, commonly used for indicating whether an entity is active, inactive, pending, or deleted.
/// </summary>
/// <remarks>
/// This enum provides a set of common status values that can be applied to various entities in the application.
/// The <see cref="DisplayAttribute"/> is used to provide user-friendly names (in Vietnamese) for each status,
/// suitable for display in a user interface.
/// </remarks>
public enum Status
{
    /// <summary>
    /// The entity is active and operational.
    /// </summary>
    [Display(Name = "Đang hoạt động")] // Vietnamese: Active
    Active = 0,

    /// <summary>
    /// The entity is inactive and not currently operational.
    /// </summary>
    [Display(Name = "Ngừng hoạt động")] // Vietnamese: Inactive
    Inactive = 1,

    /// <summary>
    /// The entity is in a pending state, awaiting some action or approval.
    /// </summary>
    [Display(Name = "Đang chờ xử lý")] // Vietnamese: Pending
    Pending = 2,

    /// <summary>
    /// The entity has been marked as deleted (often used for soft deletes).
    /// </summary>
    [Display(Name = "Đã xóa")] // Vietnamese: Deleted
    Deleted = 3
}