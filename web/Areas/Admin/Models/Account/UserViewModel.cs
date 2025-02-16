using System.ComponentModel;

namespace web.Areas.Admin.Models.Account;

public class UserViewModel
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    [DisplayName("Vai trò")] public string? RoleName { get; set; }
    public DateTime? CreatedAt { get; set; }
}