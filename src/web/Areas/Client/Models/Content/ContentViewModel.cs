using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

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
    public List<int> SelectedCategoryIds { get; set; } = new();
    public List<int> SelectedTagIds { get; set; } = new();
    public string SortBy { get; set; } = "newest";
    public PublishStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }

    // Filter options
    public List<domain.Entities.Category> Categories { get; set; } = new();
    public List<domain.Entities.Category> ParentCategories { get; set; } = new();
    public List<domain.Entities.Category> ChildCategories { get; set; } = new();
    public List<SelectListItem> CategoriesSelectList { get; set; } = new();
    public List<domain.Entities.Tag> Tags { get; set; } = new();

    // Content type
    public domain.Entities.ContentType? ContentType { get; set; }

    // Helper properties for UI
    public bool HasFilters => !string.IsNullOrEmpty(Search) ||
                             !string.IsNullOrEmpty(CategorySlug) ||
                             !string.IsNullOrEmpty(TagSlug) ||
                             SelectedCategoryIds.Any() ||
                             SelectedTagIds.Any() ||
                             Status.HasValue ||
                             FromDate.HasValue ||
                             ToDate.HasValue;

    public int StartItem => (CurrentPage - 1) * PageSize + 1;
    public int EndItem => Math.Min(StartItem + PageSize - 1, TotalContents);
}