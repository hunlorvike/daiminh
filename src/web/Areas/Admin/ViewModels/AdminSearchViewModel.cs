namespace web.Areas.Admin.ViewModels;

public class AdminSearchViewModel
{
    public string Query { get; set; } = string.Empty;
    public List<AdminSearchResultItemViewModel> Results { get; set; } = new();
    public int TotalResults => Results.Count;
}