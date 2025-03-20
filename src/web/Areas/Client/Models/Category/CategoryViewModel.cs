using System.ComponentModel;

namespace web.Areas.Client.Models.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        [DisplayName("Tên danh mục")] public string? Name { get; set; }
        [DisplayName("ID danh mục cha")] public string? ParentCategoryName { get; set; }
    }
}
