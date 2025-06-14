namespace web.Areas.Client.ViewModels;

// ViewModel đơn giản cho mỗi ảnh trong gallery
public class ProductImageClientViewModel
{
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsMain { get; set; }
}