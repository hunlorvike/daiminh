using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Admin.ViewModels;

public class AttributeValueFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Thuộc tính")]
    public int? AttributeId { get; set; }

    public List<SelectListItem> AttributeOptions { get; set; } = new();
}
