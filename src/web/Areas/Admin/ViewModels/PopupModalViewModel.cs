// Path: web.Areas.Admin.ViewModels.PopupModal
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels;

public class PopupModalViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tiêu đề Popup", Prompt = "Nhập tiêu đề hiển thị")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Nội dung", Prompt = "Nhập nội dung popup (HTML được hỗ trợ)")]
    [DataType(DataType.Html)]
    public string? Content { get; set; }

    [Display(Name = "Ảnh Popup", Prompt = "URL ảnh hiển thị trong popup")]
    [MaxLength(2048, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? ImageUrl { get; set; }

    [Display(Name = "URL liên kết", Prompt = "Địa chỉ liên kết khi click vào popup")]
    [MaxLength(2048, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? LinkUrl { get; set; }

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Ngày bắt đầu")]
    public DateTime? StartDate { get; set; }

    [Display(Name = "Ngày kết thúc")]
    public DateTime? EndDate { get; set; }
}