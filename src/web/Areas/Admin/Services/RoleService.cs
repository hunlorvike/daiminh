using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shared.Models;
using web.Areas.Admin.Services.Interfaces;
using web.Areas.Admin.ViewModels;
using X.PagedList;
using X.PagedList.EF;

namespace web.Areas.Admin.Services;

[Register(ServiceLifetime.Scoped)]
public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<RoleService> _logger;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public RoleService(
        ApplicationDbContext context,
        IMapper mapper,
        ILogger<RoleService> logger,
        RoleManager<Role> roleManager,
        UserManager<User> userManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IPagedList<RoleListItemViewModel>> GetPagedRolesAsync(
        RoleFilterViewModel filter,
        int pageNumber,
        int pageSize)
    {
        IQueryable<Role> query = _roleManager.Roles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            string lowerSearchTerm = filter.SearchTerm.Trim().ToLower();
            query = query.Where(r => r.Name != null && r.Name.ToLower().Contains(lowerSearchTerm));
        }

        query = query.OrderBy(r => r.Name);

        var pagedRoles = await query
            .ProjectTo<RoleListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);

        foreach (var roleItem in pagedRoles)
        {
            roleItem.NumberOfClaims = await _context.Set<RoleClaim>()
                                                   .Where(rc => rc.RoleId == roleItem.Id)
                                                   .CountAsync();

            roleItem.NumberOfUsers = await _context.Set<UserRole>()
                                                  .Where(ur => ur.RoleId == roleItem.Id)
                                                  .CountAsync();
        }

        return pagedRoles;
    }

    public async Task<RoleViewModel?> GetRoleByIdAsync(int id)
    {
        Role? role = await _roleManager.Roles
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(r => r.Id == id);
        if (role == null) return null;

        var viewModel = _mapper.Map<RoleViewModel>(role);

        viewModel.SelectedClaimDefinitionIds = await GetSelectedClaimDefinitionIdsForRoleAsync(id);

        return viewModel;
    }

    public async Task<List<int>> GetSelectedClaimDefinitionIdsForRoleAsync(int roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null) return new List<int>();

        var currentRoleClaims = await _roleManager.GetClaimsAsync(role);

        var allClaimDefinitions = await _context.ClaimDefinitions.AsNoTracking().ToListAsync();
        var claimDefDict = allClaimDefinitions.ToDictionary(cd => (cd.Type, cd.Value), cd => cd.Id);

        var selectedIds = new List<int>();
        foreach (var claim in currentRoleClaims)
        {
            if (claim.Type == "Permission" && claimDefDict.TryGetValue((claim.Type, claim.Value), out int claimDefId))
            {
                selectedIds.Add(claimDefId);
            }
        }
        return selectedIds;
    }

    public async Task<OperationResult<int>> CreateRoleAsync(RoleViewModel viewModel)
    {
        Role role = _mapper.Map<Role>(viewModel);

        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            // Gán claims cho vai trò mới tạo
            await SyncRoleClaimsAsync(role.Id, viewModel.SelectedClaimDefinitionIds);

            _logger.LogInformation("Created Role: ID={Id}, Name={Name}", role.Id, role.Name);
            return OperationResult<int>.SuccessResult(role.Id, $"Thêm vai trò '{role.Name}' thành công.");
        }
        else
        {
            List<string> errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Failed to create role '{Name}': {Errors}", viewModel.Name, string.Join(", ", errors));
            return OperationResult<int>.FailureResult(message: "Không thể thêm vai trò.", errors: errors);
        }
    }

    public async Task<OperationResult> UpdateRoleAsync(RoleViewModel viewModel)
    {
        Role? role = await _roleManager.FindByIdAsync(viewModel.Id.ToString());
        if (role == null)
        {
            _logger.LogWarning("Role not found for update. ID: {Id}", viewModel.Id);
            return OperationResult.FailureResult("Không tìm thấy vai trò để cập nhật.");
        }

        string oldName = role.Name ?? string.Empty;
        _mapper.Map(viewModel, role); // Cập nhật tên vai trò

        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
        {
            // Đồng bộ claims cho vai trò
            await SyncRoleClaimsAsync(role.Id, viewModel.SelectedClaimDefinitionIds);

            _logger.LogInformation("Updated Role: ID={Id}, OldName={OldName}, NewName={NewName}", role.Id, oldName, role.Name);
            return OperationResult.SuccessResult($"Cập nhật vai trò '{role.Name}' thành công.");
        }
        else
        {
            List<string> errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Failed to update role '{Name}': {Errors}", viewModel.Name, string.Join(", ", errors));
            return OperationResult.FailureResult(message: "Không thể cập nhật vai trò.", errors: errors);
        }
    }

    public async Task<OperationResult> DeleteRoleAsync(int id)
    {
        Role? role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            _logger.LogWarning("Role not found for delete. ID: {Id}", id);
            return OperationResult.FailureResult("Không tìm thấy vai trò.");
        }

        string roleName = role.Name ?? string.Empty;

        // Kiểm tra xem vai trò có đang được gán cho bất kỳ người dùng nào không
        // _userManager.GetUsersInRoleAsync(roleName) sẽ trả về danh sách người dùng.
        // Có thể dùng CountAsync trên UserRoles nếu bạn đã thêm navigation property.
        bool hasUsers = await _context.Set<UserRole>().AnyAsync(ur => ur.RoleId == id);
        if (hasUsers)
        {
            return OperationResult.FailureResult($"Không thể xóa vai trò '{roleName}' vì có người dùng đang thuộc vai trò này. Vui lòng gỡ bỏ người dùng khỏi vai trò trước khi xóa.",
                errors: new List<string> { $"Không thể xóa vai trò '{roleName}' vì có người dùng đang thuộc vai trò này. Vui lòng gỡ bỏ người dùng khỏi vai trò trước khi xóa." });
        }

        // Xóa tất cả RoleClaims liên quan trước (OnDelete: Cascade đã được cấu hình trên RoleClaim)
        // Tuy nhiên, RoleManager.DeleteAsync sẽ xử lý điều này cho chúng ta nếu các bảng được cấu hình đúng.
        var result = await _roleManager.DeleteAsync(role);

        if (result.Succeeded)
        {
            _logger.LogInformation("Deleted Role: ID={Id}, Name={Name}", id, roleName);
            return OperationResult.SuccessResult($"Xóa vai trò '{roleName}' thành công.");
        }
        else
        {
            List<string> errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Failed to delete role '{Name}': {Errors}", roleName, string.Join(", ", errors));
            return OperationResult.FailureResult(message: "Không thể xóa vai trò.", errors: errors);
        }
    }

    public async Task<List<SelectListItem>> GetAllRolesAsSelectListAsync(int? selectedId = null)
    {
        var roles = await _roleManager.Roles
                                      .AsNoTracking()
                                      .OrderBy(r => r.Name)
                                      .Select(r => new SelectListItem
                                      {
                                          Value = r.Id.ToString(),
                                          Text = r.Name,
                                          Selected = (selectedId.HasValue && r.Id == selectedId.Value)
                                      })
                                      .ToListAsync();
        roles.Insert(0, new SelectListItem { Value = "", Text = "— Tất cả vai trò —", Selected = !selectedId.HasValue || selectedId.Value == 0 });
        return roles;
    }

    private async Task SyncRoleClaimsAsync(int roleId, List<int> selectedClaimDefinitionIds)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        if (role == null) return;

        var allClaimDefinitions = await _context.ClaimDefinitions
                                                .AsNoTracking()
                                                .ToDictionaryAsync(cd => cd.Id);

        var currentRoleClaims = await _roleManager.GetClaimsAsync(role);

        var claimsToRemove = new List<System.Security.Claims.Claim>();
        foreach (var currentClaim in currentRoleClaims)
        {
            if (currentClaim.Type == "Permission")
            {
                var matchingClaimDef = allClaimDefinitions.Values
                    .FirstOrDefault(cd => cd.Type == currentClaim.Type && cd.Value == currentClaim.Value);

                if (matchingClaimDef == null || !selectedClaimDefinitionIds.Contains(matchingClaimDef.Id))
                {
                    claimsToRemove.Add(currentClaim);
                }
            }
        }

        var claimsToAdd = new List<System.Security.Claims.Claim>();
        foreach (var selectedId in selectedClaimDefinitionIds)
        {
            if (allClaimDefinitions.TryGetValue(selectedId, out var claimDef))
            {
                if (!currentRoleClaims.Any(c => c.Type == claimDef.Type && c.Value == claimDef.Value))
                {
                    claimsToAdd.Add(new System.Security.Claims.Claim(claimDef.Type, claimDef.Value));
                }
            }
            else
            {
                _logger.LogWarning("ClaimDefinition ID {ClaimId} not found when trying to add to Role {RoleId}.", selectedId, roleId);
            }
        }

        foreach (var claim in claimsToRemove)
        {
            var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
            if (!removeResult.Succeeded)
            {
                _logger.LogError("Failed to remove claim '{ClaimType}:{ClaimValue}' from role '{RoleName}': {Errors}", claim.Type, claim.Value, role.Name, string.Join(", ", removeResult.Errors.Select(e => e.Description)));
            }
        }

        foreach (var claim in claimsToAdd)
        {
            var addResult = await _roleManager.AddClaimAsync(role, claim);
            if (!addResult.Succeeded)
            {
                _logger.LogError("Failed to add claim '{ClaimType}:{ClaimValue}' to role '{RoleName}': {Errors}", claim.Type, claim.Value, role.Name, string.Join(", ", addResult.Errors.Select(e => e.Description)));
            }
        }
    }
}