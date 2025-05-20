namespace web.Areas.Admin.ViewModels;

public class RoleListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UserCount { get; set; }
}
