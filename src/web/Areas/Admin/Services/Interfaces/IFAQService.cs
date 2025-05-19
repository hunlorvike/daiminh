using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IFAQService
{
    Task<IPagedList<FAQListItemViewModel>> GetPagedFAQsAsync(FAQFilterViewModel filter, int pageNumber, int pageSize);

    Task<FAQViewModel?> GetFAQByIdAsync(int id);

    Task<OperationResult<int>> CreateFAQAsync(FAQViewModel viewModel);

    Task<OperationResult> UpdateFAQAsync(FAQViewModel viewModel);

    Task<OperationResult> DeleteFAQAsync(int id);
}