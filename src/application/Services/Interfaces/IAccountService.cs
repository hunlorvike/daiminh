using application.Dtos.Account;
using shared.Models;
using System.Security.Claims;

namespace application.Services.Interfaces;
public interface IAccountService
{
    Task<Result<List<Claim>>> LoginAsync(LoginDto loginDto);
}
