using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace application.Services;

/// <summary>
/// Service for managing roles.
/// </summary>
public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleService"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public RoleService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    /// <returns>A list of roles.</returns>
    public async Task<List<Role>> GetAllAsync()
    {
        try
        {
            // Retrieve all roles from the database.
            var roles = await _context.Roles.AsNoTracking().ToListAsync();
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