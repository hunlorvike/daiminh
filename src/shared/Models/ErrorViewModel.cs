namespace shared.Models;

public class ErrorViewModel
{
    public string RequestId { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public Exception? Exception { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
