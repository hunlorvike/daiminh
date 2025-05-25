using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IProductService
{
    Task<IPagedList<ProductListItemViewModel>> GetPagedProductsAsync(ProductFilterViewModel filter, int pageNumber, int pageSize);
    Task<ProductViewModel?> GetProductByIdAsync(int id);
    Task<OperationResult<int>> CreateProductAsync(ProductViewModel viewModel);
    Task<OperationResult> UpdateProductAsync(ProductViewModel viewModel);
    Task<OperationResult> DeleteProductAsync(int id);
    Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null);
    Task<List<SelectListItem>> GetProductCategorySelectListAsync(int? selectedValue = null);
    Task<List<SelectListItem>> GetBrandSelectListAsync(int? selectedValue = null);
    Task<List<SelectListItem>> GetAttributeSelectListAsync(List<int>? selectedValues = null);
    Task<List<SelectListItem>> GetTagSelectListAsync(List<int>? selectedValues = null);
    Task PopulateProductViewModelSelectListsAsync(ProductViewModel viewModel);
    Task<List<SelectListItem>> GetProductSelectListAsync(int? selectedValue = null);
}
