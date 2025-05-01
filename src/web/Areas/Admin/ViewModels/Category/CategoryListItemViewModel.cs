using shared.Enums;

namespace web.Areas.Admin.ViewModels.Category;

public class CategoryListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ParentName { get; set; }
    public int? ParentId { get; set; }
    public CategoryType Type { get; set; }
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; }
    public int ItemCount { get; set; } // Combined count of products, articles, etc.
    public DateTime? UpdatedAt { get; set; }
    public int Level { get; set; } // For potential indentation in view
    public string? Icon { get; set; }
    public bool HasChildren { get; set; }
}