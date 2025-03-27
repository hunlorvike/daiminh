using shared.Enums;

namespace web.Areas.Admin.ViewModels.Tag;

public class TagListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TagType Type { get; set; }
    public int ItemCount { get; set; } // Number of associated items (products, articles, etc.)
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}