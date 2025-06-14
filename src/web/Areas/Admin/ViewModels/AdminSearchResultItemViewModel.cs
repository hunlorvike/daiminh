namespace web.Areas.Admin.ViewModels;

public class AdminSearchResultItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // "Sản phẩm", "Bài viết", "Người dùng"...
    public string Icon { get; set; } = "ti-file"; // Tabler icon class
}