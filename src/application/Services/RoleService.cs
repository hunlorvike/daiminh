using application.Interfaces;
using domain.Entities;
using infrastructure;
using Microsoft.EntityFrameworkCore;

namespace application.Services;

public class RoleService : IRoleService
{
    private readonly ApplicationDbContext _context; // Inject DbContext

    public RoleService(ApplicationDbContext context) // Constructor
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllAsync()
    {
        try
        {
            // Direct query using DbContext
            var roles = await _context.Roles.ToListAsync();
            return roles;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}