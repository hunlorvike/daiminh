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
    /// A multi-line text input.
    /// </summary>
    [Display(Name = "Văn bản (nhiều dòng)")] // Vietnamese: Text (multi-line)
    TextArea = 1,

    /// <summary>
    /// A rich text editor (e.g., with formatting options).
    /// </summary>
    [Display(Name = "Văn bản (định dạng)")] // Vietnamese: Rich Text
    RichText = 2,

    /// <summary>
    /// A numeric input.
    /// </summary>
    [Display(Name = "Số")] // Vietnamese: Number
    Number = 3,

    /// <summary>
    /// A date input.
    /// </summary>
    [Display(Name = "Ngày")] // Vietnamese: Date
    Date = 4,

    /// <summary>
    /// A date and time input.
    /// </summary>
    [Display(Name = "Ngày giờ")] // Vietnamese: Date and Time
    DateTime = 5,

    /// <summary>
    /// A boolean (true/false) input, typically a checkbox.
    /// </summary>
    [Display(Name = "Đúng/Sai")] // Vietnamese: True/False
    Boolean = 6,

    /// <summary>
    /// A single-select dropdown list.
    /// </summary>
    [Display(Name = "Chọn một")] // Vietnamese: Select (single)
    Select = 7,

    /// <summary>
    /// A multi-select dropdown list or checkbox group.
    /// </summary>
    [Display(Name = "Chọn nhiều")] // Vietnamese: Select (multiple)
    MultiSelect = 8,

    /// <summary>
    /// A file upload input.
    /// </summary>
    [Display(Name = "Tệp tin")] // Vietnamese: File
    File = 9,

    /// <summary>
    /// An image upload input.
    /// </summary>
    [Display(Name = "Hình ảnh")] // Vietnamese: Image
    Image = 10,

    /// <summary>
    /// A color picker input.
    /// </summary>
    [Display(Name = "Màu sắc")] // Vietnamese: Color
    Color = 11,

    /// <summary>
    /// A URL input.
    /// </summary>
    [Display(Name = "URL")] // Vietnamese: URL
    Url = 12,

    /// <summary>
    /// An email address input.
    /// </summary>
    [Display(Name = "Email")] // Vietnamese: Email
    Email = 13,

    /// <summary>
    /// A phone number input.
    /// </summary>
    [Display(Name = "Số điện thoại")] // Vietnamese: Phone Number
    Phone = 14,

    /// <summary>
    ///  A JSON data input, allows for structured data entry.
    /// </summary>
    [Display(Name = "JSON")]
    Json = 15,

    /// <summary>
    /// A code editor input (e.g., for HTML, CSS, JavaScript).
    /// </summary>
    [Display(Name = "Mã")] // Vietnamese: Code
    Code = 16
}