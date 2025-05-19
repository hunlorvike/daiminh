using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface ISlideService
{
    Task<IPagedList<SlideListItemViewModel>> GetPagedSlidesAsync(SlideFilterViewModel filter, int pageNumber, int pageSize);

    Task<SlideViewModel?> GetSlideByIdAsync(int id);

    Task<OperationResult<int>> CreateSlideAsync(SlideViewModel viewModel);

    Task<OperationResult> UpdateSlideAsync(SlideViewModel viewModel);

    Task<OperationResult> DeleteSlideAsync(int id);
}