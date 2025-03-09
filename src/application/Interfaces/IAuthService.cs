using domain.Entities;
using shared.Models;

namespace application.Interfaces;

public interface IAuthService
{
    Task<BaseResponse> SignUpAsync(User user);
    Task<BaseResponse> SignInAsync(User user, string scheme);
    Task SignOutAsync(string scheme);
}