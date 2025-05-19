namespace web.Areas.Admin.ViewModels;

public class FAQListItemViewModel
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; }
    public DateTime? UpdatedAt { get; set; }
}