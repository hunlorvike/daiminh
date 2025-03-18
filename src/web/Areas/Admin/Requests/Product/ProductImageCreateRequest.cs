using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Product
{
    public class ProductImageCreateRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập URL hình ảnh")]
        [Display(Name = "URL hình ảnh")]
        [MaxLength(255, ErrorMessage = "URL hình ảnh không được vượt quá 255 ký tự")]
        public string ImageUrl { get; set; }

        [Display(Name = "Mô tả hình ảnh")]
        [MaxLength(255, ErrorMessage = "Mô tả hình ảnh không được vượt quá 255 ký tự")]
        public string AltText { get; set; }

        [Display(Name = "Hình ảnh chính")]
        public bool IsPrimary { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public short DisplayOrder { get; set; }
    }
}