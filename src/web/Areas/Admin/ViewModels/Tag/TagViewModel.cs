using shared.Enums;

namespace web.Areas.Admin.ViewModels.Tag;

public class TagViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TagType Type { get; set; } = TagType.Product;
}