// GalleryFilterViewModel.cs
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Gallery;

public class GalleryFilterViewModel
{
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public int? TagId { get; set; }
    public PublishStatus? Status { get; set; }
    public bool? IsFeatured { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}