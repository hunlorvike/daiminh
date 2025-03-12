namespace shared.Enums;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the publication status of content or products.
/// </summary>
/// <remarks>
/// This enum defines the possible states for content and products, such as drafts, published items,
/// archived items, items pending review, and private items. The <see cref="DisplayAttribute"/>
/// is used to provide user-friendly names (in Vietnamese) for each status, suitable for display in a UI.
/// </remarks>
public enum PublishStatus
{
    /// <summary>
    /// The item is a draft and is not yet published.
    /// </summary>
    [Display(Name = "Bản nháp")] // Vietnamese: Draft
    Draft = 0,

    /// <summary>
    /// The item is published and visible to the public (or intended audience).
    /// </summary>
    [Display(Name = "Đã xuất bản")] // Vietnamese: Published
    Published = 1,

    /// <summary>
    /// The item is archived and no longer actively displayed but is retained for historical purposes.
    /// </summary>
    [Display(Name = "Đã lưu trữ")] // Vietnamese: Archived
    Archived = 2,

    /// <summary>
    /// The item is pending review or approval before being published.
    /// </summary>
    [Display(Name = "Đang chờ duyệt")] // Vietnamese: Pending
    Pending = 3,

    /// <summary>
    /// The item is private and only visible to specific users or roles.
    /// </summary>
    [Display(Name = "Riêng tư")] // Vietnamese: Private
    Private = 4
}