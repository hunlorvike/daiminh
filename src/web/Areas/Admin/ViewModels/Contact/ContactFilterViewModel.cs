using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels.Contact;

public class ContactFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Trạng thái")]
    public ContactStatus? Status { get; set; } // null = All

    // SelectList for dropdown
    public List<SelectListItem>? StatusOptions { get; set; }
}