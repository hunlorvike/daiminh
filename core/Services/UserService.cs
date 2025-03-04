using core.Attributes;
using core.Common.Constants;
using Core.Common.Models;
using core.Entities;
using core.Interfaces;
using core.Interfaces.Infrastructure;
using core.Interfaces.Service;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace core.Services;

public class UserService(IUnitOfWork unitOfWork) : ScopedService, IUserService
{
    public async Task<List<User>> GetAllAsync()
    {
        try
        {
            var userRepository = unitOfWork.GetRepository<User, int>();
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

    public async Task<User?> GetByIdAsync(int id)
    {
        try
        {
            var userRepository = unitOfWork.GetRepository<User, int>();

            return await userRepository
                .Where(u => u.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<BaseResponse> AddAsync(User user)
    {
        try
        {
            var userRepository = unitOfWork.GetRepository<User, int>();
            var roleRepository = unitOfWork.GetRepository<Role, int>();

            var existingUsers = await userRepository
                .Where(u => u.Username == user.Username || u.Email == user.Email)
                .ToListAsync();

            var errors = new Dictionary<string, string[]>();

            if (existingUsers.Any(u => u.Username == user.Username))
                errors.Add(nameof(user.Username), ["Tên người dùng đã tồn tại."]);

            if (existingUsers.Any(u => u.Email == user.Email)) errors.Add(nameof(user.Email), ["Email đã tồn tại."]);

            if (errors.Count != 0) return new ErrorResponse(errors);

            var defaultRole = await roleRepository
                                  .Where(r => r.Name == RoleConstants.User)
                                  .FirstOrDefaultAsync() ??
                              throw new Exception("Role mặc định không tồn tại. Vui lòng chạy seeding cho role.");
            user.PasswordHash = BC.HashPassword(user.PasswordHash);
            user.RoleId = defaultRole.Id;

            await userRepository.AddAsync(user);
            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<User>(user, "Đăng ký thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }

    public async Task<BaseResponse> UpdateAsync(int id, User user)
    {
        try
        {
            var userRepository = unitOfWork.GetRepository<User, int>();

            var existingUser = await userRepository
                .FirstOrDefaultAsync(u => u.Id == id);

            if (existingUser == null)
                return new ErrorResponse(new Dictionary<string, string[]>
                {
                    { "General", ["Người dùng không tồn tại"] }
                });

            existingUser.RoleId = user.RoleId;

            await unitOfWork.SaveChangesAsync();

            return new SuccessResponse<User>(existingUser, "Cập nhật thành công.");
        }
        catch (Exception ex)
        {
            return new ErrorResponse(new Dictionary<string, string[]>
            {
                { "General", [ex.Message] }
            });
        }
    }
}