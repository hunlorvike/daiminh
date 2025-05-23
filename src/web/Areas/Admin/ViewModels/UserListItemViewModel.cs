namespace web.Areas.Admin.ViewModels;

public class UserListItemViewModel
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public bool IsActive { get; set; }
    public List<string> RoleNames { get; set; } = new List<string>();
    public DateTime? LastLoginDate { get; set; }
}