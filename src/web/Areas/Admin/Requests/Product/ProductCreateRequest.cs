using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.Requests.Product;

public class ProductCreateRequest
{
    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
    [Display(Name = "Tên sản phẩm")]
    [MaxLength(255, ErrorMessage = "Tên sản phẩm không được vượt quá 255 ký tự")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập đường dẫn")]
    [Display(Name = "Đường dẫn")]
    [MaxLength(255, ErrorMessage = "Đường dẫn không được vượt quá 255 ký tự")]
    [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "Đường dẫn chỉ được chứa chữ thường, số và dấu gạch ngang")]
    public string? Slug { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập giá cơ bản")]
    [Display(Name = "Giá cơ bản")]
    [Range(0, 9999999999, ErrorMessage = "Giá cơ bản phải lớn hơn hoặc bằng 0")]
    public decimal BasePrice { get; set; }

    [Display(Name = "Mã SKU")]
    [MaxLength(50, ErrorMessage = "Mã SKU không được vượt quá 50 ký tự")]
    public string? Sku { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
    [Display(Name = "Trạng thái")]
    public PublishStatus Status { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm")]
    [Display(Name = "Loại sản phẩm")]
    public int ProductTypeId { get; set; }

    // SEO properties
    [Display(Name = "Tiêu đề Meta")]
    [MaxLength(255, ErrorMessage = "Tiêu đề Meta không được vượt quá 255 ký tự")]
    public string? MetaTitle { get; set; }

    [Display(Name = "Mô tả Meta")]
    public string? MetaDescription { get; set; }

    [Display(Name = "Đường dẫn chuẩn")]
    [MaxLength(255, ErrorMessage = "Đường dẫn chuẩn không được vượt quá 255 ký tự")]
    public string? CanonicalUrl { get; set; }

    [Display(Name = "Tiêu đề Open Graph")]
    [MaxLength(255, ErrorMessage = "Tiêu đề Open Graph không được vượt quá 255 ký tự")]
    public string? OgTitle { get; set; }

    [Display(Name = "Mô tả Open Graph")]
    public string? OgDescription { get; set; }

    [Display(Name = "Hình ảnh Open Graph")]
    [MaxLength(255, ErrorMessage = "Hình ảnh Open Graph không được vượt quá 255 ký tự")]
    public string? OgImage { get; set; }

    [Display(Name = "Dữ liệu có cấu trúc")]
    public string? StructuredData { get; set; }

    // Related collections
    [Display(Name = "Danh mục")]
    public List<int> CategoryIds { get; set; } = new List<int>();

    [Display(Name = "Thẻ")]
    public List<int> TagIds { get; set; } = new List<int>();

    [Display(Name = "Hình ảnh")]
    public List<ProductImageCreateRequest> Images { get; set; } = new List<ProductImageCreateRequest>();

    [Display(Name = "Thông tin bổ sung")]
    public List<ProductFieldValueCreateRequest> CustomFields { get; set; } = new List<ProductFieldValueCreateRequest>();
}