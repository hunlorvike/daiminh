using X.PagedList; // Cần thêm package X.PagedList

namespace web.Areas.Client.ViewModels;

public class BrandDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? Website { get; set; }

    // Sử dụng IPagedList để chứa danh sách sản phẩm có phân trang
    public IPagedList<ProductCardViewModel> Products { get; set; } = default!;
}