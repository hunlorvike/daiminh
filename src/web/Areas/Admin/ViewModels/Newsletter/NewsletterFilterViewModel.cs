using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Admin.ViewModels.Newsletter;

public class NewsletterFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Trạng thái")]
    public bool? IsActive { get; set; } // Nullable for "All" option

    public List<SelectListItem> StatusOptions { get; set; } = new();
}