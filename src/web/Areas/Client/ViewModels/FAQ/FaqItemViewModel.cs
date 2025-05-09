namespace web.Areas.Client.ViewModels.FAQ;

public class FAQItemViewModel
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}