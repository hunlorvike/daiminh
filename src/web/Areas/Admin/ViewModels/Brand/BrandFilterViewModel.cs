using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Admin.ViewModels.Brand;

public class BrandFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Trạng thái")]
    public bool? IsActive { get; set; }

    public List<SelectListItem> StatusOptions { get; set; } = new();
}