using shared.Enums;

namespace web.Areas.Admin.ViewModels.Category;

public class CategoryListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? ImageUrl { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; }
    public CategoryType Type { get; set; }
    public int ItemCount { get; set; } // Number of associated items (products, articles, etc.)
    public int ChildrenCount { get; set; } // Number of child categories
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}