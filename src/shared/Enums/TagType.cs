using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum TagType : ushort
{
    [Display(Name = "Sản phẩm")]
    Product,

    [Display(Name = "Bài viết")]
    Article,
}