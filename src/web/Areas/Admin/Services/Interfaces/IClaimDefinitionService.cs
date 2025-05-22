using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IClaimDefinitionService
{
    Task<IPagedList<ClaimDefinitionListItemViewModel>> GetPagedClaimDefinitionsAsync(
        ClaimDefinitionFilterViewModel filter,
        int pageNumber,
        int pageSize);

    Task<ClaimDefinitionViewModel?> GetClaimDefinitionByIdAsync(int id);

    Task<OperationResult<int>> CreateClaimDefinitionAsync(ClaimDefinitionViewModel viewModel);

    Task<OperationResult> UpdateClaimDefinitionAsync(ClaimDefinitionViewModel viewModel);

    Task<OperationResult> DeleteClaimDefinitionAsync(int id);

    Task<List<ClaimDefinitionViewModel>> GetAllClaimDefinitionsAsync();
    Task<bool> IsClaimDefinitionValueUniqueAsync(int? id, string value);

}