using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IBannerService
{
    Task<IPagedList<BannerListItemViewModel>> GetPagedBannersAsync(BannerFilterViewModel filter, int pageNumber, int pageSize);

    Task<BannerViewModel?> GetBannerByIdAsync(int id);

    Task<OperationResult<int>> CreateBannerAsync(BannerViewModel viewModel);

    Task<OperationResult> UpdateBannerAsync(BannerViewModel viewModel);

    Task<OperationResult> DeleteBannerAsync(int id);
}
