using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Product;

public class ProductFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Danh mục")]
    public int? CategoryId { get; set; }

    [Display(Name = "Thương hiệu")]
    public int? BrandId { get; set; }

    [Display(Name = "Trạng thái")]
    public PublishStatus? Status { get; set; }

    [Display(Name = "Sản phẩm nổi bật")]
    public bool? IsFeatured { get; set; }

    public List<SelectListItem> CategoryOptions { get; set; } = new();
    public List<SelectListItem> BrandOptions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> IsFeaturedOptions { get; set; } = new();
}