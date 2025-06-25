namespace web.Areas.Client.ViewModels;

public class HomeViewModel
{
    public List<BannerViewModel> HeroBanners { get; set; } = new();
    public List<ProductCardViewModel> FeaturedProducts { get; set; } = new();
    public List<ArticleCardViewModel> LatestArticles { get; set; } = new();
}

public class BannerViewModel
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? LinkUrl { get; set; }
}

public class ProductCardViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
}

public class ArticleCardViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public DateTime PublishedAt { get; set; }
    public string? CategoryName { get; set; }
}