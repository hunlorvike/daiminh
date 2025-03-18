using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Models.Product
{
    public class ProductImageViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sản phẩm")]
        public int ProductId { get; set; }

        [Display(Name = "URL hình ảnh")]
        public string ImageUrl { get; set; }

        [Display(Name = "Mô tả hình ảnh")]
        public string AltText { get; set; }

        [Display(Name = "Hình ảnh chính")]
        public bool IsPrimary { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public short DisplayOrder { get; set; }
    }
}