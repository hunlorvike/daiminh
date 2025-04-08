using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Areas.Admin.ViewModels.FAQ;

public class FAQViewModel
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty; // For Summernote
    public int OrderIndex { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    // For Many-to-Many relationship with Category
    public List<int> SelectedCategoryIds { get; set; } = new List<int>();

    // --- Dropdown Data ---
    public SelectList? CategoryList { get; set; } // Multi-select handled in View

}
