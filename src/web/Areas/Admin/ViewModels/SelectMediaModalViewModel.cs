namespace web.Areas.Admin.ViewModels;

public class SelectMediaModalViewModel
{
    public List<MediaFileViewModel> Files { get; set; } = new();
    public MediaFilterViewModel Filter { get; set; } = new();
}