namespace shared.Enums;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the status of a contact request or inquiry.
/// </summary>
/// <remarks>
/// This enum provides a set of predefined values to track the progress of a contact,
/// from initial submission to completion or marking as spam.  The `[Display]` attribute
/// is used to provide user-friendly names for each status value, suitable for display
/// in a user interface.
/// </remarks>
public enum ContactStatus
{
    /// <summary>
    /// The contact request has been received but has not yet been processed.
    /// </summary>
    [Display(Name = "Đang chờ xử lý")] // English: "Pending processing"
    Pending = 0,

    /// <summary>
    /// The contact request is currently being processed.
    /// </summary>
    [Display(Name = "Đang xử lý")] // English: "Being processed"
    InProgress = 1,

    /// <summary>
    /// The contact request has been processed and completed.
    /// </summary>
    [Display(Name = "Hoàn thành")] // English: "Completed"
    Completed = 2,

    /// <summary>
    /// The contact request has been marked as spam.
    /// </summary>
    [Display(Name = "Spam")] // English: "Spam"
    Spam = 3
}