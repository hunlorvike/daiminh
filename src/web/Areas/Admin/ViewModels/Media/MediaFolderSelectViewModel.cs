namespace web.Areas.Admin.ViewModels.Media;

public class MediaFolderSelectViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public int Level { get; set; } // For indentation in dropdowns
    public string DisplayName => new string('-', Level * 2) + " " + Name;
}