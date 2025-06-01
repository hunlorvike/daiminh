using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IPopupModalService
{
    Task<IPagedList<PopupModalListItemViewModel>> GetPagedPopupModalsAsync(PopupModalFilterViewModel filter, int pageNumber, int pageSize);
    Task<PopupModalViewModel?> GetPopupModalByIdAsync(int id);
    Task<OperationResult<int>> CreatePopupModalAsync(PopupModalViewModel viewModel);
    Task<OperationResult> UpdatePopupModalAsync(PopupModalViewModel viewModel);
    Task<OperationResult> DeletePopupModalAsync(int id);
}