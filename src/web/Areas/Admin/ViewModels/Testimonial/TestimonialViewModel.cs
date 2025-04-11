using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Testimonial;

public class TestimonialViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tên khách hàng", Prompt = "Nhập tên đầy đủ")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string ClientName { get; set; } = string.Empty;

    [Display(Name = "Chức danh", Prompt = "Ví dụ: Giám đốc, Trưởng phòng")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? ClientTitle { get; set; }

    [Display(Name = "Công ty", Prompt = "Tên công ty của khách hàng")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? ClientCompany { get; set; }

    [Display(Name = "Ảnh đại diện (MinIO Path)", Prompt = "Chọn hoặc nhập đường dẫn ảnh")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? ClientAvatar { get; set; }

    [Display(Name = "Nội dung đánh giá", Prompt = "Nhập nội dung phản hồi của khách hàng")]
    [Required(ErrorMessage = "{0} không được để trống")]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Đánh giá (sao)", Prompt = "Chọn số sao đánh giá")]
    [Range(1, 5, ErrorMessage = "{0} phải từ {1} đến {2} sao")]
    public int Rating { get; set; } = 5;

    [Display(Name = "Hiển thị", Prompt = "Chọn để hiển thị đánh giá này")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Thứ tự hiển thị", Prompt = "Nhập số thứ tự (số nhỏ hiển thị trước)")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm")]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Tham chiếu dự án", Prompt = "Tên hoặc mã dự án liên quan (nếu có)")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự")]
    public string? ProjectReference { get; set; }
}