namespace web.Areas.Admin.ViewModels.Media;

public class MediaFolderViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? ParentId { get; set; }

    // For display purposes
    public IEnumerable<MediaFolderSelectViewModel>? AvailableParents { get; set; }
}