using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IArticleService
{
    Task<IPagedList<ArticleListItemViewModel>> GetPagedArticlesAsync(ArticleFilterViewModel filter, int pageNumber, int pageSize);

    Task<ArticleViewModel?> GetArticleByIdAsync(int id);

    Task<OperationResult<int>> CreateArticleAsync(ArticleViewModel viewModel, string? authorId, string? authorName);

    Task<OperationResult> UpdateArticleAsync(ArticleViewModel viewModel);

    Task<OperationResult> DeleteArticleAsync(int id);

    Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null);
    Task<List<SelectListItem>> GetArticleSelectListAsync(List<int>? selectedValue = null);
}
