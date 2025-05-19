using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IPageService
{
    Task<IPagedList<PageListItemViewModel>> GetPagedPagesAsync(PageFilterViewModel filter, int pageNumber, int pageSize);

    Task<PageViewModel?> GetPageByIdAsync(int id);

    Task<OperationResult<int>> CreatePageAsync(PageViewModel viewModel);

    Task<OperationResult> UpdatePageAsync(PageViewModel viewModel);

    Task<OperationResult> DeletePageAsync(int id);

    Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null);
}