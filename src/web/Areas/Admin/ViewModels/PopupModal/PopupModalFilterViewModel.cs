using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Admin.ViewModels.PopupModal;

public class PopupModalFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Trạng thái kích hoạt")]
    public bool? IsActive { get; set; }

    public List<SelectListItem> ActiveStatusOptions { get; set; } = new();
}