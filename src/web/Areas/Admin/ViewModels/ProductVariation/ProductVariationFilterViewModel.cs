using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Admin.ViewModels.ProductVariation;

public class ProductVariationFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Đang hoạt động")]
    public bool? IsActive { get; set; }

    [Display(Name = "Là mặc định")]
    public bool? IsDefault { get; set; }

    [Display(Name = "Còn hàng")]
    public bool? InStock { get; set; }

    public List<SelectListItem> IsActiveOptions { get; set; } = new();
    public List<SelectListItem> IsDefaultOptions { get; set; } = new();
    public List<SelectListItem> InStockOptions { get; set; } = new();
}