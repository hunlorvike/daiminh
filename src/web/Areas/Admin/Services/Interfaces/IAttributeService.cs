using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IAttributeService
{
    Task<IPagedList<AttributeListItemViewModel>> GetPagedAttributesAsync(AttributeFilterViewModel filter, int pageNumber, int pageSize);

    Task<AttributeViewModel?> GetAttributeByIdAsync(int id);

    Task<OperationResult<int>> CreateAttributeAsync(AttributeViewModel viewModel);

    Task<OperationResult> UpdateAttributeAsync(AttributeViewModel viewModel);

    Task<OperationResult> DeleteAttributeAsync(int id);

    Task<bool> IsSlugUniqueAsync(string slug, int? ignoreId = null);

    Task<bool> HasRelatedValuesAsync(int attributeId);

    Task<List<SelectListItem>> GetAttributeSelectListAsync(int? selectedValue = null);
    Task<List<SelectListItem>> GetAttributeSelectListAsync(List<int>? selectedValue = null);
}
