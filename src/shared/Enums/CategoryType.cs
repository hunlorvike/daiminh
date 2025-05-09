using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum CategoryType : ushort
{
    [Display(Name = "Sản phẩm")]
    Product,

    [Display(Name = "Bài viết")]
    Article,

    [Display(Name = "Câu hỏi")]
    FAQ
}