namespace web.Areas.Admin.ViewModels;

public class AttributeValueListItemViewModel
{
    public int Id { get; set; }
    public int AttributeId { get; set; }
    public string AttributeName { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public int OrderIndex { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
