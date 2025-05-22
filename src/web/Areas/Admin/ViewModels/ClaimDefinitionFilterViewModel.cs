using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class ClaimDefinitionFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }
}
