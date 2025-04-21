namespace web.Areas.Admin.ViewModels.Media;

public class UploadResponseViewModel
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public MediaFileViewModel? File { get; set; }

}
