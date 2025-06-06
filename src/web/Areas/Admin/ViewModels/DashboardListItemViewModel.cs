namespace web.Areas.Admin.ViewModels;

public class DashboardListItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; } // Ví dụ: Danh mục, Ngày tạo
    public string? DateInfo { get; set; }
    public string Url { get; set; } = string.Empty;
    public int? Value { get; set; } // Ví dụ: Lượt xem, Số lượng
    public string? ValueLabel { get; set; } // Ví dụ: "lượt xem"
}
