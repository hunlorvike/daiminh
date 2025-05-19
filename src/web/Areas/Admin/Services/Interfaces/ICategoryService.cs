using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface ICategoryService
{
    Task<IPagedList<CategoryListItemViewModel>> GetPagedCategoriesAsync(CategoryFilterViewModel filter, int pageNumber, int pageSize);

    Task<CategoryViewModel?> GetCategoryByIdAsync(int id);

    Task<OperationResult<int>> CreateCategoryAsync(CategoryViewModel viewModel);

    Task<OperationResult> UpdateCategoryAsync(CategoryViewModel viewModel);

    Task<OperationResult> DeleteCategoryAsync(int id);

    Task<List<SelectListItem>> GetParentCategorySelectListAsync(CategoryType categoryType, int? selectedValue = null, int? excludeCategoryId = null);

    Task<bool> IsSlugUniqueAsync(string slug, CategoryType type, int? ignoreId = null);

    Task<(bool hasChildren, int itemCount, CategoryType type, string name)> CheckCategoryRelationsAsync(int categoryId);

}