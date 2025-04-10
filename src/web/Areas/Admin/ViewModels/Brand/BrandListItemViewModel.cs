namespace web.Areas.Admin.ViewModels.Brand;

public class BrandListItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LogoUrl { get; set; } // MinIO Path
    public string? Website { get; set; }
    public bool IsActive { get; set; }
    public int ProductCount { get; set; }
    public DateTime UpdatedAt { get; set; } // Or CreatedAt if preferred
}