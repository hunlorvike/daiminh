using core.Entities.Shared;

namespace core.Entities.CMS;

public class Page : ActivatableEntity
{
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Content { get; set; }
    public string? Template { get; set; }
    public int DisplayOrder { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
}