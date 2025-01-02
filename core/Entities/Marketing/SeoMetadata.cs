using core.Entities.Shared;

namespace core.Entities.Marketing;

public class SeoMetadata : BaseEntity
{
    public string EntityType { get; set; } = null!;
    public Guid EntityId { get; set; }
    public string? TwitterTitle { get; set; }
    public string? TwitterDescription { get; set; }
    public string? TwitterImage { get; set; }
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string? StructuredData { get; set; }
    public string? CanonicalUrl { get; set; }
    public string? Robots { get; set; }
    public decimal? Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}