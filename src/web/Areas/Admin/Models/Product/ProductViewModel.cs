using shared.Enums;
using System.ComponentModel.DataAnnotations;
using web.Areas.Admin.Models.Category;
using web.Areas.Admin.Models.Tag;

namespace web.Areas.Admin.Models.Product;

public class ProductViewModel
{
    public int Id { get; set; }

    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; }

    [Display(Name = "Đường dẫn")]
    public string Slug { get; set; }

    [Display(Name = "Mô tả")]
    public string Description { get; set; }

    [Display(Name = "Giá cơ bản")]
    public decimal BasePrice { get; set; }

    [Display(Name = "Mã SKU")]
    public string Sku { get; set; }

    [Display(Name = "Trạng thái")]
    public PublishStatus Status { get; set; }

    [Display(Name = "Loại sản phẩm")]
    public int ProductTypeId { get; set; }

    [Display(Name = "Loại sản phẩm")]
    public string ProductTypeName { get; set; }

    [Display(Name = "Hình ảnh chính")]
    public string PrimaryImage { get; set; }

    [Display(Name = "Ngày tạo")]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Ngày cập nhật")]
    public DateTime? UpdatedAt { get; set; }

    // SEO properties
    public string MetaTitle { get; set; }
    public string MetaDescription { get; set; }
    public string CanonicalUrl { get; set; }
    public string OgTitle { get; set; }
    public string OgDescription { get; set; }
    public string OgImage { get; set; }
    public string StructuredData { get; set; }

    // Related collections
    public List<ProductImageViewModel> Images { get; set; } = new List<ProductImageViewModel>();
    public List<ProductFieldValueViewModel> CustomFields { get; set; } = new List<ProductFieldValueViewModel>();
    public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
}