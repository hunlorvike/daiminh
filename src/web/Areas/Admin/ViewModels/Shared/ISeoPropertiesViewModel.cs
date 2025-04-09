namespace web.Areas.Admin.ViewModels.Shared;

public interface ISeoPropertiesViewModel
{
    string? MetaTitle { get; set; }
    string? MetaDescription { get; set; }
    string? MetaKeywords { get; set; }

    string? CanonicalUrl { get; set; }
    bool NoIndex { get; set; }
    bool NoFollow { get; set; }

    string? OgTitle { get; set; }
    string? OgDescription { get; set; }
    string? OgImage { get; set; }
    string? OgType { get; set; }

    string? TwitterTitle { get; set; }
    string? TwitterDescription { get; set; }
    string? TwitterImage { get; set; }
    string? TwitterCard { get; set; }

    string? SchemaMarkup { get; set; }
    string? BreadcrumbJson { get; set; }

    double SitemapPriority { get; set; }
    string? SitemapChangeFrequency { get; set; }
}