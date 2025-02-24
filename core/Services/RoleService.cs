using core.Attributes;
using core.Entities;
using core.Interfaces;
using core.Interfaces.Infrastructure;
using core.Interfaces.Service;

namespace core.Services;

public class RoleService(IUnitOfWork unitOfWork) : ScopedService, IRoleService
{
    public async Task<List<Role>> GetAllAsync()
    {
        try
        {
            var roleRepository = unitOfWork.GetRepository<Role, int>();
            var roles = await roleRepository
                .ToListAsync();

            return roles;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}