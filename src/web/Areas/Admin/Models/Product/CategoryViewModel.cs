using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Models.Product
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }

        [Display(Name = "Đường dẫn")]
        public string Slug { get; set; }
    }
}