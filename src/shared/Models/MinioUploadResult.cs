namespace shared.Models;
public class MinioUploadResult
{
    public string ObjectName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string GeneratedFileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
    public string PublicUrl { get; set; } = string.Empty;

}
