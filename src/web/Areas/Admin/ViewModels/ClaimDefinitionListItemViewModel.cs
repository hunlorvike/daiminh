namespace web.Areas.Admin.ViewModels;

public class ClaimDefinitionListItemViewModel
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
