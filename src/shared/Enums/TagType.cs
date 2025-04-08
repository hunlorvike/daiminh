using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum TagType
{
    [Display(Name = "Sản phẩm")]
    Product,

    [Display(Name = "Bài viết")]
    Article,

    [Display(Name = "Dự án")]
    Project,

    [Display(Name = "Thư viện ảnh")]
    Gallery
}
