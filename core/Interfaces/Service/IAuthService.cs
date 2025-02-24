using Core.Common.Models;
using core.Entities;

namespace core.Interfaces.Service;

public interface IAuthService
{
    Task<BaseResponse> SignUpAsync(User user);
    Task<BaseResponse> SignInAsync(User user, string scheme);
    Task SignOutAsync(string scheme);
}