namespace web.Areas.Client.ViewModels.Article;

public class ArticleSideBarViewModel{
    public List<CategorySidebarViewModel> ArticleCategories { get; set; } = new();
    public List<ArticleItemViewModel> PopularArticles { get; set; } = new();
    public List<TagSidebarViewModel> PopularTags { get; set; } = new();
}

public class CategorySidebarViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int ArticleCount { get; set; }
    public string? Icon { get; set; } 
}

public class TagSidebarViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int ArticleCount { get; set; }
}