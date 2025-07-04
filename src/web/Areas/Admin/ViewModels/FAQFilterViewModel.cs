using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Admin.ViewModels;

public class FAQFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Danh mục")]
    public int? CategoryId { get; set; }

    // This will hold the list of categories for the dropdown
    public List<SelectListItem>? Categories { get; set; }
}