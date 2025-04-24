using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Product;

public class ProductFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; } // Name, ShortDescription, Manufacturer, etc.

    [Display(Name = "Trạng thái")]
    public PublishStatus? Status { get; set; }

    [Display(Name = "Danh mục")]
    public int? CategoryId { get; set; }

    [Display(Name = "Thương hiệu")]
    public int? BrandId { get; set; }

    [Display(Name = "Nổi bật")]
    public bool? IsFeatured { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool? IsActive { get; set; }

    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> CategoryOptions { get; set; } = new();
    public List<SelectListItem> BrandOptions { get; set; } = new();
    public List<SelectListItem> IsFeaturedOptions { get; set; } = new(); // Yes/No/All
    public List<SelectListItem> IsActiveOptions { get; set; } = new(); // True/False/All

}
