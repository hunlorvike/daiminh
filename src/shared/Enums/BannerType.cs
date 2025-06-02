using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum BannerType
{
    [Display(Name = "Đầu trang")] Header,
    [Display(Name = "Chân trang")] Footer,
    [Display(Name = "Thanh bên")] Sidebar,
    [Display(Name = "Trang chủ")] Homepage,
    [Display(Name = "Danh mục")] Category,
    [Display(Name = "Chi tiết sản phẩm")] ProductDetail,
    [Display(Name = "Deal/Flash Sale")] Deal,
    [Display(Name = "Slide chính")] Slide,
    [Display(Name = "Trang tìm kiếm")] Search,
}
