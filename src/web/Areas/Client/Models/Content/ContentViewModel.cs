using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Client.Models.Content;

public class ContentViewModel
{
    // Content items
    public List<domain.Entities.Content> Contents { get; set; } = new();

    // Pagination properties
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalPages { get; set; }
    public int TotalContents { get; set; }

    // Filter properties
    public string Search { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;
    public string TagSlug { get; set; } = string.Empty;
    public string[] CategorySlugs { get; set; } = Array.Empty<string>();
    public string[] TagSlugs { get; set; } = Array.Empty<string>();

    // Filter options
    public List<domain.Entities.Category> Categories { get; set; } = new();
    public List<SelectListItem> CategoriesSelectList { get; set; } = new();
    public List<string> Tags { get; set; } = new();

    // Content type
    public domain.Entities.ContentType? ContentType { get; set; }

    // Helper properties for UI
    public bool HasFilters => !string.IsNullOrEmpty(Search) ||
                             !string.IsNullOrEmpty(CategorySlug) ||
                             !string.IsNullOrEmpty(TagSlug) ||
                             (CategorySlugs != null && CategorySlugs.Length > 0) ||
                             (TagSlugs != null && TagSlugs.Length > 0);
}