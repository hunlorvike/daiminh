using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Client.ViewModels;

public class ProductFilterViewModel
{
    [Display(Name = "Tìm kiếm sản phẩm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Danh mục")]
    public int? CategoryId { get; set; }

    [Display(Name = "Thương hiệu")]
    public int? BrandId { get; set; }

    [Display(Name = "Sắp xếp theo")]
    public string? SortBy { get; set; }

    // Dùng để đổ dữ liệu vào các dropdown trong View
    public List<SelectListItem> Categories { get; set; } = new();
    public List<SelectListItem> Brands { get; set; } = new();
    public List<SelectListItem> SortOptions { get; set; } = new();
}