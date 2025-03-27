namespace web.Areas.Admin.ViewModels.FAQ;

public class FAQListItemViewModel
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
