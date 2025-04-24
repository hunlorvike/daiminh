namespace web.Areas.Admin.ViewModels.Media;

public class SelectMediaModalViewModel
{
    public List<MediaFileViewModel> Files { get; set; } = new();
    public MediaFilterViewModel Filter { get; set; } = new();
}