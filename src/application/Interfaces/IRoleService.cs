using domain.Entities;

namespace application.Interfaces;

public interface IRoleService
{
    Task<List<Role>> GetAllAsync();
}