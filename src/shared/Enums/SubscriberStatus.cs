namespace shared.Enums;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the status of a subscriber, such as pending confirmation, subscribed, unsubscribed, or bounced.
/// </summary>
/// <remarks>
/// This enum defines the possible states for a subscriber in a mailing list or subscription system.
/// The <see cref="DisplayAttribute"/> is used to provide user-friendly names (in Vietnamese) for each status,
/// suitable for display in a user interface.
/// </remarks>
public enum SubscriberStatus
{
    /// <summary>
    /// The subscriber has signed up but has not yet confirmed their subscription (e.g., via email confirmation).
    /// </summary>
    [Display(Name = "Đang chờ xác nhận")] // Vietnamese: Pending
    Pending = 0,

    /// <summary>
    /// The subscriber has confirmed their subscription and is actively receiving emails.
    /// </summary>
    [Display(Name = "Đã đăng ký")] // Vietnamese: Subscribed
    Subscribed = 1,

    /// <summary>
    /// The subscriber has unsubscribed from the mailing list.
    /// </summary>
    [Display(Name = "Đã hủy đăng ký")] // Vietnamese: Unsubscribed
    Unsubscribed = 2,

    /// <summary>
    /// Emails sent to the subscriber have bounced (returned as undeliverable).
    /// </summary>
    [Display(Name = "Bị trả lại")] // Vietnamese: Bounced
    Bounced = 3
}