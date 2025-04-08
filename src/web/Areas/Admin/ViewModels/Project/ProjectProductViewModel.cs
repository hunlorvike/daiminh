namespace web.Areas.Admin.ViewModels.Project;

public class ProjectProductViewModel
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; } // For display
    public string? ProductImageUrl { get; set; } // For display
    public string? Usage { get; set; } // How the product was used in this project
    public int OrderIndex { get; set; } = 0;
    public bool IsDeleted { get; set; } = false; // For removing association
}
