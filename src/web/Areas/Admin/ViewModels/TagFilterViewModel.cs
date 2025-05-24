using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels;

public class TagFilterViewModel
{
    [Display(Name = "Loại thẻ")]
    public TagType? Type { get; set; }

    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    public List<SelectListItem>? TagTypes { get; set; }
}