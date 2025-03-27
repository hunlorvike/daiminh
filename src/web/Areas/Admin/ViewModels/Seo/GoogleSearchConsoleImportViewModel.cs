using System.ComponentModel.DataAnnotations;

namespace web.Areas.Admin.ViewModels.Seo;

public class GoogleSearchConsoleImportViewModel
{
    [Required(ErrorMessage = "Vui lòng chọn file JSON từ Google Search Console")]
    [Display(Name = "File JSON từ Google Search Console")]
    public IFormFile JsonFile { get; set; } = null!;

    [Display(Name = "Ngày bắt đầu")]
    [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
    public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-30);

    [Display(Name = "Ngày kết thúc")]
    [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc")]
    public DateTime EndDate { get; set; } = DateTime.Now;

    [Display(Name = "Ghi đè dữ liệu hiện có")]
    public bool OverwriteExistingData { get; set; } = false;
}
