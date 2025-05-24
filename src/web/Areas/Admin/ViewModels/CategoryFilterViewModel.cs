using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

namespace web.Areas.Admin.ViewModels;

public class CategoryFilterViewModel
{
    [Display(Name = "Tìm kiếm")]
    public string? SearchTerm { get; set; }

    [Display(Name = "Loại danh mục")]
    public CategoryType? Type { get; set; }

    [Display(Name = "Trạng thái")]
    public bool? IsActive { get; set; }

    public List<SelectListItem> TypeOptions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
}