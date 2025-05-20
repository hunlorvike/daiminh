using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class RoleViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tên Vai trò", Prompt = "Nhập tên vai trò")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [StringLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression("^[a-zA-Z0-9_.-]*$", ErrorMessage = "Tên vai trò chỉ chấp nhận chữ cái, số và một số ký tự đặc biệt cơ bản.")]
    public string Name { get; set; } = string.Empty;
}
