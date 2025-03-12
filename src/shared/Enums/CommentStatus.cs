namespace shared.Enums;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the status of a comment, indicating whether it's pending review, approved, marked as spam, or rejected.
/// </summary>
/// <remarks>
/// This enum provides a set of predefined values for managing comment moderation. The <see cref="DisplayAttribute"/>
/// is used to provide user-friendly names for each status, suitable for display in a user interface.
/// </remarks>
public enum CommentStatus
{
    /// <summary>
    /// The comment is awaiting moderation and is not yet visible publicly.
    /// </summary>
    [Display(Name = "Đang chờ duyệt")] // English: "Pending processing"
    Pending = 0,

    /// <summary>
    /// The comment has been approved by a moderator and is visible publicly.
    /// </summary>
    [Display(Name = "Đã duyệt")]
    Approved = 1,

    /// <summary>
    /// The comment has been flagged as spam and is not visible.
    /// </summary>
    [Display(Name = "Spam")] // English: "Spam"
    Spam = 2,

    /// <summary>
    /// The comment has been rejected by a moderator and is not visible.
    /// </summary>
    [Display(Name = "Bị từ chối")]
    Rejected = 3
}