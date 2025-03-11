using shared.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Models.Slider;

public class SliderViewModel
{
    public int Id { get; set; }
    [DisplayName("Tiêu đề")]
    public string Title { get; set; }

    [DisplayName("Hình ảnh")]
    public string ImageUrl { get; set; }

    [DisplayName("Đường dẫn liên kết")]
    public string? LinkUrl { get; set; }

    [DisplayName("Thứ tự hiển thị")]
    public int Order { get; set; }

    [DisplayName("Nội dung HTML overlay")]
    public string? OverlayHtml { get; set; }

    [DisplayName("Vị trí overlay")]
    public OverlayPosition? OverlayPosition { get; set; }
    [Display(Name = "Ngày tạo")] public DateTime? CreatedAt { get; set; }
}