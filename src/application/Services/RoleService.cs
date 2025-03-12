using application.Interfaces;
using domain.Entities;
using shared.Interfaces;

namespace application.Services;

public class RoleService(IUnitOfWork unitOfWork) : IRoleService
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