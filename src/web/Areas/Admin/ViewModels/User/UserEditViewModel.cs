using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace web.Areas.Admin.ViewModels.User;

public class UserEditViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Required(ErrorMessage = "{0} không được để trống")]
    [Display(Name = "Tên đăng nhập", Prompt = "Nhập tên đăng nhập")]
    [MaxLength(50, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    [RegularExpression("^[a-zA-Z0-9_.-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái, số, dấu gạch dưới, dấu chấm, dấu gạch ngang")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "{0} không được để trống")]
    [Display(Name = "Địa chỉ Email", Prompt = "Nhập địa chỉ email")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    [EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ")]
    public string Email { get; set; } = string.Empty;
}