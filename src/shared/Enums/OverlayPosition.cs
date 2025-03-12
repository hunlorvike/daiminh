namespace shared.Enums;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the position of an overlay element on a target element.
/// </summary>
/// <remarks>
/// This enum defines a set of predefined positions for placing an overlay relative to another element,
/// such as an image or a container.  The <see cref="DisplayAttribute"/> is used to provide user-friendly
/// names (in Vietnamese) for each position, suitable for display in a user interface.
/// </remarks>
public enum OverlayPosition
{
    /// <summary>
    /// Top-left corner of the target element.
    /// </summary>
    [Display(Name = "Góc trên bên trái")] // Vietnamese: Top-left corner
    TopLeft = 0,

    /// <summary>
    /// Top-center of the target element.
    /// </summary>
    [Display(Name = "Ở giữa trên")] // Vietnamese: Top-center
    TopCenter = 1,

    /// <summary>
    /// Top-right corner of the target element.
    /// </summary>
    [Display(Name = "Góc trên bên phải")] // Vietnamese: Top-right corner
    TopRight = 2,

    /// <summary>
    /// Center-left of the target element.
    /// </summary>
    [Display(Name = "Ở giữa bên trái")] // Vietnamese: Center-left
    CenterLeft = 3,

    /// <summary>
    /// Center of the target element.
    /// </summary>
    [Display(Name = "Ở giữa")] // Vietnamese: Center
    Center = 4,

    /// <summary>
    /// Center-right of the target element.
    /// </summary>
    [Display(Name = "Ở giữa bên phải")] // Vietnamese: Center-right
    CenterRight = 5,

    /// <summary>
    /// Bottom-left corner of the target element.
    /// </summary>
    [Display(Name = "Góc dưới bên trái")] // Vietnamese: Bottom-left corner
    BottomLeft = 6,

    /// <summary>
    /// Bottom-center of the target element.
    /// </summary>
    [Display(Name = "Ở giữa dưới")] // Vietnamese: Bottom-center
    BottomCenter = 7,

    /// <summary>
    /// Bottom-right corner of the target element.
    /// </summary>
    [Display(Name = "Góc dưới bên phải")] // Vietnamese: Bottom-right corner
    BottomRight = 8
}