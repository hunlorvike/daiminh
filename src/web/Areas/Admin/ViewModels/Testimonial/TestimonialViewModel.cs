using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Testimonial;

public class TestimonialViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tên khách hàng", Prompt = "Nhập tên đầy đủ của khách hàng")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string ClientName { get; set; } = string.Empty;

    [Display(Name = "Chức danh", Prompt = "Ví dụ: Giám đốc, Trưởng phòng,... (không bắt buộc)")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? ClientTitle { get; set; }

    [Display(Name = "Công ty", Prompt = "Tên công ty của khách hàng (không bắt buộc)")]
    [MaxLength(100, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? ClientCompany { get; set; }

    [Display(Name = "Ảnh đại diện", Prompt = "URL hoặc đường dẫn tới ảnh đại diện")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? ClientAvatar { get; set; }

    [Display(Name = "Nội dung đánh giá", Prompt = "Nhập nội dung phản hồi của khách hàng")]
    [Required(ErrorMessage = "{0} không được để trống.")]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Xếp hạng (1-5 sao)", Prompt = "Chọn mức độ hài lòng")]
    [Range(1, 5, ErrorMessage = "{0} phải từ {1} đến {2}.")]
    public int Rating { get; set; } = 5;

    [Display(Name = "Hiển thị")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Thứ tự hiển thị", Prompt = "Nhập số thứ tự (số nhỏ hiển thị trước)")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm.")]
    public int OrderIndex { get; set; } = 0;

    [Display(Name = "Tham chiếu dự án", Prompt = "Ví dụ: Tên dự án, ID dự án,... (không bắt buộc)")]
    [MaxLength(255, ErrorMessage = "{0} không được vượt quá {1} ký tự.")]
    public string? ProjectReference { get; set; }
}