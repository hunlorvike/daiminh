using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using shared.Enums;
using web.Areas.Admin.ViewModels.Shared;

namespace web.Areas.Admin.ViewModels;

public class PageViewModel : SeoViewModel
{
    [HiddenInput(DisplayValue = false)]
    public int Id { get; set; }

    [Display(Name = "Tiêu đề trang", Prompt = "Nhập tiêu đề trang")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Slug (URL)", Prompt = "phan-url-than-thien-trang")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "{0} chỉ được chứa chữ cái thường, số và dấu gạch ngang.")]
    public string Slug { get; set; } = string.Empty;

    [Display(Name = "Nội dung", Prompt = "Nhập nội dung chi tiết trang")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [DataType(DataType.Html)]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Trạng thái")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public PublishStatus Status { get; set; } = PublishStatus.Draft;

    [Display(Name = "Ngày xuất bản")]
    public DateTime? PublishedAt { get; set; }
}