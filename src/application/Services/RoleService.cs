using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace application.Services;

/// <summary>
/// Service for managing roles.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RoleService"/> class.
/// </remarks>
/// <param name="context">The application database context.</param>
public class RoleService(ApplicationDbContext context) : IRoleService
{

    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    /// <returns>A list of roles.</returns>
    public async Task<List<Role>> GetAllAsync()
    {
        try
        {
            // Retrieve all roles from the database.
            var roles = await context.Roles.AsNoTracking().ToListAsync();
            return roles;
        }
        catch (Exception ex)
        {
            //log
            //_logger.LogError(ex, "GetAllAsync RoleService");
            throw new Exception("Đã xảy ra lỗi khi lấy danh sách vai trò.", ex);
        }
    }
}