namespace web.Areas.Admin.ViewModels.FAQ;

public class FAQViewModel
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int OrderIndex { get; set; }
    public bool IsActive { get; set; } = true;

    // For display purposes
    public IEnumerable<FAQCategoryViewModel>? Categories { get; set; }
}