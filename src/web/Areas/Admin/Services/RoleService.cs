using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoRegister;
using domain.Entities;
using Microsoft.AspNetCore.Identity;
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
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ILogger<RoleService> _logger;

    public RoleService(
        RoleManager<Role> roleManager,
        UserManager<User> userManager,
        IMapper mapper,
        ILogger<RoleService> logger)
    {
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IPagedList<RoleListItemViewModel>> GetPagedRolesAsync(string? searchTerm, int pageNumber, int pageSize)
    {
        IQueryable<Role> query = _roleManager.Roles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string lowerSearchTerm = searchTerm.Trim().ToLower();
            query = query.Where(r => r.Name != null && r.Name.ToLower().Contains(lowerSearchTerm));
        }

        query = query.OrderBy(r => r.Name);

        return await query
            .ProjectTo<RoleListItemViewModel>(_mapper.ConfigurationProvider)
            .ToPagedListAsync(pageNumber, pageSize);
    }
    public async Task<List<RoleListItemViewModel>> GetAllRolesAsync()
    {
        return await _roleManager.Roles
            .AsNoTracking()
            .OrderBy(r => r.Name)
            .ProjectTo<RoleListItemViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }


    public async Task<RoleViewModel?> GetRoleByIdAsync(int id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        return _mapper.Map<RoleViewModel>(role);
    }

    public async Task<OperationResult<int>> CreateRoleAsync(RoleViewModel viewModel)
    {
        if (await RoleNameExistsAsync(viewModel.Name))
        {
            return OperationResult<int>.FailureResult($"Tên vai trò '{viewModel.Name}' đã tồn tại.");
        }

        var role = new Role { Name = viewModel.Name };

        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            _logger.LogInformation("Created Role: ID={Id}, Name={Name}", role.Id, role.Name);
            return OperationResult<int>.SuccessResult(role.Id, $"Tạo vai trò '{role.Name}' thành công.");
        }
        else
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Lỗi khi tạo vai trò {Name}: {Errors}", viewModel.Name, string.Join("; ", errors));
            return OperationResult<int>.FailureResult(errors.FirstOrDefault() ?? "Lỗi khi tạo vai trò.", errors);
        }
    }

    public async Task<OperationResult> UpdateRoleAsync(RoleViewModel viewModel)
    {
        var role = await _roleManager.FindByIdAsync(viewModel.Id.ToString());
        if (role == null)
        {
            return OperationResult.FailureResult("Không tìm thấy vai trò.");
        }

        if (!role.Name!.Equals(viewModel.Name, StringComparison.OrdinalIgnoreCase) &&
            await RoleNameExistsAsync(viewModel.Name, viewModel.Id))
        {
            return OperationResult.FailureResult($"Tên vai trò '{viewModel.Name}' đã tồn tại.");
        }

        role.Name = viewModel.Name;

        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
        {
            _logger.LogInformation("Updated Role: ID={Id}, Name={Name}", role.Id, role.Name);
            return OperationResult.SuccessResult($"Cập nhật vai trò '{role.Name}' thành công.");
        }
        else
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Lỗi khi cập nhật vai trò ID {Id}: {Errors}", viewModel.Id, string.Join("; ", errors));
            return OperationResult.FailureResult(errors.FirstOrDefault() ?? "Lỗi khi cập nhật vai trò.", errors);
        }
    }

    public async Task<OperationResult> DeleteRoleAsync(int id)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        if (role == null)
        {
            return OperationResult.FailureResult("Không tìm thấy vai trò.");
        }

        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
        if (usersInRole.Any())
        {
            _logger.LogWarning("Attempt to delete role '{RoleName}' (ID: {RoleId}) which is in use by {UserCount} user(s).", role.Name, role.Id, usersInRole.Count);
            return OperationResult.FailureResult($"Không thể xóa vai trò '{role.Name}' vì đang có người dùng thuộc vai trò này.");
        }

        if (role.Name?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true && role.Id == 1)
        {
            _logger.LogWarning("Attempt to delete primary Admin role (ID: {RoleId}).", role.Id);
            return OperationResult.FailureResult("Không thể xóa vai trò Quản trị viên gốc.");
        }


        var roleName = role.Name;
        var result = await _roleManager.DeleteAsync(role);

        if (result.Succeeded)
        {
            _logger.LogInformation("Deleted Role: ID={Id}, Name={Name}", id, roleName);
            return OperationResult.SuccessResult($"Xóa vai trò '{roleName}' thành công.");
        }
        else
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogError("Lỗi khi xóa vai trò ID {Id}: {Errors}", id, string.Join("; ", errors));
            return OperationResult.FailureResult(errors.FirstOrDefault() ?? $"Không thể xóa vai trò '{roleName}'.", errors);
        }
    }

    public async Task<bool> RoleNameExistsAsync(string name, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(name)) return false;
        var role = await _roleManager.FindByNameAsync(name);
        if (role == null) return false;
        if (ignoreId.HasValue && role.Id == ignoreId.Value) return false;
        return true;
    }

    public async Task<List<string>> GetUserRolesAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return new List<string>();
        return (await _userManager.GetRolesAsync(user)).ToList();
    }
}