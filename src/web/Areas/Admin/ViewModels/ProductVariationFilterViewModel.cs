using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Admin.ViewModels;

public class ProductVariationFilterViewModel
{
    [HiddenInput]
    public int ProductId { get; set; }


    [Display(Name = "Trạng thái hoạt động")]
    public bool? IsActive { get; set; }

    [Display(Name = "Là mặc định")]
    public bool? IsDefault { get; set; }

    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> IsDefaultOptions { get; set; } = new();
}