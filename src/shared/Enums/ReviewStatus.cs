namespace shared.Enums;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the status of a review, indicating whether it is pending approval, approved, rejected, or marked as spam.
/// </summary>
/// <remarks>
/// This enum provides a set of predefined values for managing the moderation of reviews. The <see cref="DisplayAttribute"/>
/// is used to provide user-friendly names (in Vietnamese) for each status, suitable for display in a user interface.
/// </remarks>
public enum ReviewStatus
{
    /// <summary>
    /// The review is awaiting moderation and is not yet visible.
    /// </summary>
    [Display(Name = "Đang chờ duyệt")] // Vietnamese: Pending
    Pending = 0,

    /// <summary>
    /// The review has been approved by a moderator and is visible.
    /// </summary>
    [Display(Name = "Đã duyệt")] // Vietnamese: Approved
    Approved = 1,

    /// <summary>
    /// The review has been rejected by a moderator and is not visible.
    /// </summary>
    [Display(Name = "Bị từ chối")] // Vietnamese: Rejected
    Rejected = 2,

    /// <summary>
    /// The review has been flagged as spam and is not visible.
    /// </summary>
    [Display(Name = "Spam")] // Vietnamese: Spam
    Spam = 3
}