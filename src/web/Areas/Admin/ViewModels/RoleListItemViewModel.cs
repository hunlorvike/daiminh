namespace web.Areas.Admin.ViewModels;

public class RoleListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int NumberOfClaims { get; set; }
    public int NumberOfUsers { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
