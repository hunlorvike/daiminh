namespace web.Areas.Admin.ViewModels;

public class SelectMediaModalViewModel
{
    public List<MediaFileViewModel> Files { get; set; } = default!;
    public int TotalItems => Files?.Count ?? 0;
}