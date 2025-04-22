using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Tag;

public class TagFilterViewModel
{
    [Display(Name = "Loại thẻ")]
    public TagType? Type { get; set; }

    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    public List<SelectListItem>? TagTypes { get; set; }
}
