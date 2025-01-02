using core.Common.Enums;
using core.Entities.Shared;

namespace core.Entities.Users;

public class User : ActivatableEntity
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; } = UserRole.Customer;
}