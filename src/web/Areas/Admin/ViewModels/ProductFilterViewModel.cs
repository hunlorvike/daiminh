using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels;

public class ProductFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Danh mục")]
    public int? CategoryId { get; set; }

    [Display(Name = "Trạng thái xuất bản")]
    public PublishStatus? Status { get; set; }

    [Display(Name = "Trạng thái kích hoạt")]
    public bool? IsActive { get; set; }

    [Display(Name = "Nổi bật")]
    public bool? IsFeatured { get; set; }

    public List<SelectListItem> CategoryOptions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> ActiveOptions { get; set; } = new();
    public List<SelectListItem> FeaturedOptions { get; set; } = new();
}