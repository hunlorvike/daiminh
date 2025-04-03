namespace web.Areas.Admin.ViewModels.Category;

public class CategorySelectViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public int Level { get; set; }
    public string DisplayName => new string('-', Level * 2) + " " + Name;
}