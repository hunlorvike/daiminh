using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IRoleService
{
    Task<IPagedList<RoleListItemViewModel>> GetPagedRolesAsync(
        RoleFilterViewModel filter,
        int pageNumber,
        int pageSize);

    Task<RoleViewModel?> GetRoleByIdAsync(int id);

    Task<List<int>> GetSelectedClaimDefinitionIdsForRoleAsync(int roleId);

    Task<OperationResult<int>> CreateRoleAsync(RoleViewModel viewModel);

    Task<OperationResult> UpdateRoleAsync(RoleViewModel viewModel);

    Task<OperationResult> DeleteRoleAsync(int id);

    Task<List<SelectListItem>> GetAllRolesAsSelectListAsync(int? selectedId = null);
}