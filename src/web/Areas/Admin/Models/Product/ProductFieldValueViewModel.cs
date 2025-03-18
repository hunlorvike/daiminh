using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Models.Product
{
    public class ProductFieldValueViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sản phẩm")]
        public int ProductId { get; set; }

        [Display(Name = "Trường")]
        public int FieldId { get; set; }

        [Display(Name = "Tên trường")]
        public string FieldName { get; set; }

        [Display(Name = "Loại trường")]
        public string FieldType { get; set; }

        [Display(Name = "Giá trị")]
        public string Value { get; set; }

        public List<string> Options { get; set; } = new List<string>();
    }
}