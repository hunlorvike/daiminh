namespace application.Dtos.Shared;

public class SeoDto : BaseDto
{
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public bool NoIndex { get; set; }
    public bool NoFollow { get; set; }
    public string? OgTitle { get; set; }
    public string? OgDescription { get; set; }
    public string? OgImage { get; set; }
    public string? OgType { get; set; }
    public string? TwitterTitle { get; set; }
    public string? TwitterDescription { get; set; }
    public string? TwitterImage { get; set; }
    public string? TwitterCard { get; set; }
    public string? SchemaMarkup { get; set; }
    public string? BreadcrumbJson { get; set; }
    public double? SitemapPriority { get; set; }
    public string? SitemapChangeFrequency { get; set; }
}
