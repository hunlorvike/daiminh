using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Product;

/// <summary>
/// Represents a request to delete a product.
/// </summary>
public class ProductDeleteRequest
{
    /// <summary>
    /// Gets or sets the ID of the product to delete.
    /// </summary>
    [Required(ErrorMessage = "ID là bắt buộc.")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product (for display purposes only).
    /// </summary>
    public string Name { get; set; } = string.Empty;
}