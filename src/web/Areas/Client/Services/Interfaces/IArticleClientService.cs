using web.Areas.Client.ViewModels;
using X.PagedList;

namespace web.Areas.Client.Services.Interfaces;

public interface IArticleClientService
{
    Task<IPagedList<ArticleCardViewModel>> GetArticlesAsync(int pageNumber, int pageSize);
    Task<ArticleDetailViewModel?> GetArticleBySlugAsync(string slug);
}