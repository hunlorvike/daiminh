using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

/// <summary>
/// Represents the type of an entity, indicating whether it's a product or a service.
/// </summary>
/// <remarks>
/// Thís enum provides a set of predefined values for managing entity types. The <see cref="DisplayAttribute"/>
/// </remarks>
public enum EntityType
{
    /// <summary>
    /// The entity is a product.
    /// </summary>
    [Display(Name = "Sản phẩm")] // English: "Product"
    Product = 0,

    /// <summary>
    /// The entity is a content.
    /// </summary>
    [Display(Name = "Bài viết")] // English: "Content"
    Content = 1
}
