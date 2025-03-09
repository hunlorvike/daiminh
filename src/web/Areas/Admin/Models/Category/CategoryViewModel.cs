using System.ComponentModel;

namespace web.Areas.Admin.Models.Category;

public class CategoryViewModel
{
    public int Id { get; set; }
    [DisplayName("Tên danh mục")] public string? Name { get; set; }
    [DisplayName("Đường dẫn")] public string? Slug { get; set; }
    [DisplayName("Ngày tạo")] public DateTime? CreatedAt { get; set; }
    [DisplayName("ID danh mục cha")] public string? ParentCategoryName { get; set; }
}