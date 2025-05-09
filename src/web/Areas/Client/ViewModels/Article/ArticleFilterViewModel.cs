using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Client.ViewModels.Article;

public class ArticleFilterViewModel
{
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public int? TagId { get; set; }
    public List<SelectListItem> CategoryOptions { get; set; } = new();
}