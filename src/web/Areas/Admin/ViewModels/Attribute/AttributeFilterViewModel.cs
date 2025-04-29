using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Attribute;

public class AttributeFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }
}
