using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Testimonial;

public class TestimonialFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; } // Search by name, company, content

    [Display(Name = "Trạng thái")]
    public bool? IsActive { get; set; } // null = All, true = Active, false = Inactive

    [Display(Name = "Xếp hạng")]
    public int? Rating { get; set; } // null = All, 1-5

    // SelectLists for dropdowns
    public List<SelectListItem>? StatusOptions { get; set; }
    public List<SelectListItem>? RatingOptions { get; set; }
}