using System.ComponentModel.DataAnnotations;
namespace web.Areas.Admin.ViewModels.Product;
public class ProductVariantViewModel
{
    public int Id { get; set; } // 0 for new variants

    [Display(Name = "Tên biến thể", Prompt = "Ví dụ: Màu Xanh - 5L")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "SKU", Prompt = "Mã định danh kho duy nhất")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    [Display(Name = "Giá bán", Prompt = "Nhập giá bán cho biến thể")]
    [Required(ErrorMessage = "{0} không được để trống")]
    [Range(0, double.MaxValue, ErrorMessage = "{0} phải là số không âm")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; } = 0;

    [Display(Name = "Số lượng tồn kho", Prompt = "Nhập số lượng hiện có")]
    [Range(0, int.MaxValue, ErrorMessage = "{0} phải là số không âm")]
    public int StockQuantity { get; set; } = 0;

    [Display(Name = "Màu sắc", Prompt = "Ví dụ: Xanh dương, Đỏ")]
    [MaxLength(50)]
    public string? Color { get; set; }

    [Display(Name = "Kích thước/Dung tích", Prompt = "Ví dụ: 5L, 10kg, 18L")]
    [MaxLength(50)]
    public string? Size { get; set; }

    [Display(Name = "Đóng gói", Prompt = "Ví dụ: Lon, Bao, Thùng")]
    [MaxLength(50)]
    public string? Packaging { get; set; }

    [Display(Name = "Ảnh riêng (MinIO Path)", Prompt = "Đường dẫn ảnh riêng nếu khác sản phẩm chính")]
    [MaxLength(2048)]
    public string? ImageUrl { get; set; } // MinIO Path

    [Display(Name = "Hoạt động")]
    public bool IsActive { get; set; } = true;

    // Not displayed, used internally for Edit logic
    public bool IsDeleted { get; set; } = false;
}