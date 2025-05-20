using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IProductService
{
    Task<IPagedList<ProductListItemViewModel>> GetPagedProductsAsync(ProductFilterViewModel filter, int pageNumber, int pageSize);

    Task<ProductViewModel?> GetProductByIdAsync(int id);

    Task<ProductViewModel> GetProductViewModelForCreateAsync();

    Task<OperationResult<int>> CreateProductAsync(ProductViewModel viewModel, string authorId, string authorName); // Include author info

    Task<OperationResult> UpdateProductAsync(ProductViewModel viewModel);

    Task<OperationResult> DeleteProductAsync(int id);

    Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null);

    Task<List<SelectListItem>> GetProductSelectListAsync(int? selectedValue = null);

    Task<List<SelectListItem>> GetProductSelectListAsync(List<int>? selectedValues = null);
}
