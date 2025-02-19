using core.Entities;
using core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace core.Services;

public class UserService(IUnitOfWork unitOfWork)
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<User>> FindAllUsersAsync()
    {
        try
        {
            var userRepository = _unitOfWork.GetRepository<User, int>();
            var users = await userRepository
                .Include(u => u.Role)
                .ToListAsync();

            return users;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}