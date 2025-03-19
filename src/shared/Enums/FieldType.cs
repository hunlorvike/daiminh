namespace shared.Enums;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the type of a field used for content or product data.  This enum defines
/// the various input types that can be used to capture and store information.
/// </summary>
/// <remarks>
/// This enum provides a set of predefined field types, allowing for consistent data handling and UI rendering.
/// The <see cref="DisplayAttribute"/> is used to provide user-friendly names (in Vietnamese) for each field type,
/// making them suitable for display in a user interface, such as a form builder or data editor.
/// </remarks>
public enum FieldType
{
    /// <summary>
    /// A single-line text input.
    /// </summary>
    [Display(Name = "Văn bản (một dòng)")] // Vietnamese: Text (single-line)
    Text = 0,

    /// <summary>
    /// A numeric input.
    /// </summary>
    [Display(Name = "Số")] // Vietnamese: Number
    Number = 1,

    /// <summary>
    /// A date input.
    /// </summary>
    [Display(Name = "Ngày")] // Vietnamese: Date
    Date = 2,

    /// <summary>
    /// A single-select dropdown list.
    /// </summary>
    [Display(Name = "Chọn một")] // Vietnamese: Select (single)
    Select = 3,
}