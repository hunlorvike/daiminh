using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IBrandService
{
    Task<IPagedList<BrandListItemViewModel>> GetPagedBrandsAsync(BrandFilterViewModel filter, int pageNumber, int pageSize);
    Task<BrandViewModel?> GetBrandByIdAsync(int id);
    Task<OperationResult<int>> CreateBrandAsync(BrandViewModel viewModel);
    Task<OperationResult> UpdateBrandAsync(BrandViewModel viewModel);
    Task<OperationResult> DeleteBrandAsync(int id);
    Task<List<SelectListItem>> GetBrandSelectListAsync(int? selectedValue = null);
    Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null);
    Task<bool> HasRelatedProductsAsync(int brandId);
}
