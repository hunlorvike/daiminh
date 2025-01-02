using core.Common.Enums;
using core.Entities.Shared;

namespace core.Entities.CMS;

public class Post : ActivatableEntity
{
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public Guid CategoryId { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public PostStatus Status { get; set; }
    public DateTime? PublishedAt { get; set; }

    public Category Category { get; set; } = null!;
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}