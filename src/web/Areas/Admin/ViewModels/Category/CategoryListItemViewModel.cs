using shared.Enums;

namespace web.Areas.Admin.ViewModels.Category;

public class CategoryListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } // MinIO Path
    public string? Icon { get; set; }
    public string? ParentName { get; set; } // Name of the parent category
    public CategoryType Type { get; set; }
    public bool IsActive { get; set; }
    public int OrderIndex { get; set; }
    public int ChildrenCount { get; set; }
    public int ItemCount { get; set; } // Count of associated Products/Articles etc.
    public DateTime UpdatedAt { get; set; }
}