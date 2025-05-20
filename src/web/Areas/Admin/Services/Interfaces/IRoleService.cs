using shared.Models;
using web.Areas.Admin.ViewModels;
using X.PagedList;

namespace web.Areas.Admin.Services.Interfaces;

public interface IRoleService
{
    Task<IPagedList<RoleListItemViewModel>> GetPagedRolesAsync(string? searchTerm, int pageNumber, int pageSize);
    Task<RoleViewModel?> GetRoleByIdAsync(int id);
    Task<OperationResult<int>> CreateRoleAsync(RoleViewModel viewModel);
    Task<OperationResult> UpdateRoleAsync(RoleViewModel viewModel);
    Task<OperationResult> DeleteRoleAsync(int id);
    Task<bool> RoleNameExistsAsync(string name, int? ignoreId = null);
    Task<List<string>> GetUserRolesAsync(int userId);
    Task<List<RoleListItemViewModel>> GetAllRolesAsync();
}
