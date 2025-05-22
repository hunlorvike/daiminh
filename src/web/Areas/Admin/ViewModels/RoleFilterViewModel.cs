using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class RoleFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }
}