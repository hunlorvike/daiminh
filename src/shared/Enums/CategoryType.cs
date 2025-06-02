using System.ComponentModel.DataAnnotations;

namespace shared.Enums;

public enum CategoryType
{
    [Display(Name = "Sản phẩm")] Product = 0,
    [Display(Name = "Bài viết")] Article = 1,
    [Display(Name = "Câu hỏi")] FAQ = 2
}