using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Models.Product
{
    public class ProductTypeViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tên loại sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Đường dẫn")]
        public string Slug { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdatedAt { get; set; }

        // Related collections
        public List<ProductFieldDefinitionViewModel> FieldDefinitions { get; set; } = new List<ProductFieldDefinitionViewModel>();
    }
}