using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum BannerType : ushort
{
    [Display(Name = "Đầu trang")]
    Header = 1,

    [Display(Name = "Chân trang")]
    Footer = 2,

    [Display(Name = "Thanh bên")]
    Sidebar = 3,

    [Display(Name = "Trang chủ")]
    Homepage = 4,

    [Display(Name = "Danh mục")]
    Category = 5,

    [Display(Name = "Chi tiết sản phẩm")]
    ProductDetail = 6,

    [Display(Name = "Deal/Flash Sale")]
    Deal = 7,

    [Display(Name = "Popup")]
    Popup = 8,

    [Display(Name = "Slide chính")]
    Slide = 9,

    [Display(Name = "Mobile Only")]
    Mobile = 10,

    [Display(Name = "App Only")]
    AppExclusive = 11,

    [Display(Name = "Khuyến mãi/Voucher")]
    Voucher = 12,

    [Display(Name = "Trang tìm kiếm")]
    Search = 13,

    [Display(Name = "Trang thanh toán")]
    Checkout = 14,

    [Display(Name = "Trang thành công")]
    OrderSuccess = 15
}
