using System.ComponentModel;

namespace web.Areas.Admin.Models.ProductType;

public class ProductTypeViewModel
{
    public int Id { get; set; }
    [DisplayName("Loại sản phẩm")] public string? Name { get; set; }
    [DisplayName("Đường dẫn")] public string? Slug { get; set; }
    [DisplayName("Ngày tạo")] public DateTime? CreatedAt { get; set; }
}