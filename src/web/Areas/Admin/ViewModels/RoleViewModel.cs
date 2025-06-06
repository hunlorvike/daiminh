using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class RoleViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tên Vai trò", Prompt = "Nhập tên vai trò (ví dụ: Administrator)")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(256, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Quyền hạn được gán")]
    public List<int> SelectedClaimDefinitionIds { get; set; } = new List<int>();

    public List<SelectListItem>? AvailableClaimDefinitions { get; set; }
}