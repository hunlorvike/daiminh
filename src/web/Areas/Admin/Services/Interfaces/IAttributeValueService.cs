using domain.Entities;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IAttributeValueService
{
    Task<IPagedList<AttributeValueListItemViewModel>> GetPagedAttributeValuesAsync(AttributeValueFilterViewModel filter, int pageNumber, int pageSize);

    Task<AttributeValueViewModel?> GetAttributeValueByIdAsync(int id);

    Task<OperationResult<int>> CreateAttributeValueAsync(AttributeValueViewModel viewModel);

    Task<OperationResult> UpdateAttributeValueAsync(AttributeValueViewModel viewModel);

    Task<OperationResult<int>> DeleteAttributeValueAsync(int id);

    Task<bool> IsSlugUniqueAsync(string slug, int attributeId, int? ignoreId = null);

    Task<bool> IsUsedInProductVariationsAsync(int attributeValueId);

    Task<AttributeValue?> GetAttributeValueEntityWithParentAsync(int id);
}
