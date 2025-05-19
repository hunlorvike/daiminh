using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class SlideFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Trạng thái kích hoạt")]
    public bool? IsActive { get; set; }

    public List<SelectListItem> ActiveStatusOptions { get; set; } = new();
}