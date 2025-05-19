using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class BannerFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Loại Banner")]
    public BannerType? Type { get; set; }

    [Display(Name = "Trạng thái kích hoạt")]
    public bool? IsActive { get; set; }

    public List<SelectListItem> TypeOptions { get; set; } = new();
    public List<SelectListItem> ActiveStatusOptions { get; set; } = new();
}