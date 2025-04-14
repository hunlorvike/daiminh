namespace web.ViewModels.Project;

public class ProjectProductLinkViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } // Product's main image path
    public string? Usage { get; set; } // How it was used in *this* project
}