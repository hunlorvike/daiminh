using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class ArticleFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Danh mục")]
    public int? CategoryId { get; set; }

    [Display(Name = "Trạng thái")]
    public PublishStatus? Status { get; set; }

    [Display(Name = "Bài viết nổi bật")]
    public bool? IsFeatured { get; set; }

    public List<SelectListItem> CategoryOptions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> IsFeaturedOptions { get; set; } = new();
}