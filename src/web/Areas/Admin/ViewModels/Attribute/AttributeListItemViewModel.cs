namespace web.Areas.Admin.ViewModels.Attribute;

public class AttributeListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int ValueCount { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
