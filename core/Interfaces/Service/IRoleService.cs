using core.Entities;

namespace core.Interfaces.Service;

public interface IRoleService
{
    Task<List<Role>> GetAllAsync();
}