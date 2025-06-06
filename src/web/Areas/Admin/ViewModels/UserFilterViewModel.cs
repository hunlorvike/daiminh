using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class UserFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Trạng thái")]
    public bool? IsActiveFilter { get; set; }

    [Display(Name = "Vai trò")]
    public int? RoleIdFilter { get; set; }

    public List<SelectListItem>? AvailableRoles { get; set; }
}
