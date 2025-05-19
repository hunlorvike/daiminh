using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class MediaFilterViewModel
{
    [Display(Name = "Loại Media")]
    public MediaType? MediaType { get; set; }

    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    public List<SelectListItem> MediaTypeOptions { get; set; } = new();
}