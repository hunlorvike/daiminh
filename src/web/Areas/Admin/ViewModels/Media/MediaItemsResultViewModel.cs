namespace web.Areas.Admin.ViewModels.Media;

public class MediaItemsResultViewModel
{
    public List<MediaItemViewModel> Items { get; set; } = new List<MediaItemViewModel>();
    public List<BreadcrumbItemViewModel> Breadcrumbs { get; set; } = new List<BreadcrumbItemViewModel>();
    public int? CurrentFolderId { get; set; }
}

public class BreadcrumbItemViewModel
{
    public int? Id { get; set; } // Null for root
    public string Name { get; set; } = string.Empty;
}
