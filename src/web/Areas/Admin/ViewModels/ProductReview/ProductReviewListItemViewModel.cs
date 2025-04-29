using shared.Enums;

namespace web.Areas.Admin.ViewModels.ProductReview;

public class ProductReviewListItemViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public int Rating { get; set; }
    public string ContentSummary { get; set; } = string.Empty; // Truncated content
    public ReviewStatus Status { get; set; }
    public DateTime? CreatedAt { get; set; }
}
