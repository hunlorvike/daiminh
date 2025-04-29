using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.AttributeValue;

public class AttributeValueFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Thuộc tính")]
    public int? AttributeId { get; set; }

    public List<SelectListItem> AttributeOptions { get; set; } = new();
}
