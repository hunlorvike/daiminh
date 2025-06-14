namespace web.Areas.Client.ViewModels;

// ViewModel chính cho trang kết quả tìm kiếm
public class ClientSearchViewModel
{
    public string Query { get; set; } = string.Empty;

    // Tái sử dụng các ViewModel Card đã tạo trước đó
    public List<ProductCardViewModel> ProductResults { get; set; } = new();
    public List<ArticleCardViewModel> ArticleResults { get; set; } = new();

    public int ProductCount => ProductResults.Count;
    public int ArticleCount => ArticleResults.Count;
    public int TotalCount => ProductCount + ArticleCount;
}