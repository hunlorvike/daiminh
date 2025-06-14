using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shared.Enums;

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

    [Display(Name = "Trang hiển thị", Prompt = "Nhập các trang hoặc mẫu URL, cách nhau bởi dấu phẩy. Ví dụ: HomePage, /san-pham/*, /bai-viet/chi-tiet-abc, AllPages")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(1000, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string TargetPages { get; set; } = "AllPages";

    [Display(Name = "Tần suất hiển thị")]
    [Required(ErrorMessage = "Vui lòng chọn {0}.")]
    public DisplayFrequency DisplayFrequency { get; set; } = DisplayFrequency.Always;

    [Display(Name = "Thứ tự ưu tiên", Prompt = "Số nhỏ hơn ưu tiên cao hơn")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Thời gian trễ (giây)", Prompt = "Số giây trễ trước khi hiển thị")]
    [Range(0, 300, ErrorMessage = "{0} phải từ {1} đến {2} giây.")]
    public int DelaySeconds { get; set; } = 0;

    [Display(Name = "Kích hoạt")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Ngày bắt đầu")]
    public DateTime? StartDate { get; set; }

    [Display(Name = "Ngày kết thúc")]
    public DateTime? EndDate { get; set; }

    public List<SelectListItem>? DisplayFrequencyOptions { get; set; }
}