namespace web.Areas.Client.ViewModels;

public class FAQCategoryViewModel
{
    public string CategoryName { get; set; } = string.Empty;
    public string? CategoryDescription { get; set; }
    public List<FAQItemViewModel> Faqs { get; set; } = new();
}