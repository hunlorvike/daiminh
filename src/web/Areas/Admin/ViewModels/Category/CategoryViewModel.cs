using shared.Enums;

namespace web.Areas.Admin.ViewModels.Category;

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? ImageUrl { get; set; }
    public int? ParentId { get; set; }
    public int OrderIndex { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public CategoryType Type { get; set; } = CategoryType.Product;

    // SEO properties
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public bool NoIndex { get; set; }

    // For display purposes
    public IEnumerable<CategorySelectViewModel>? AvailableParents { get; set; }
}