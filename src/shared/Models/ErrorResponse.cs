using System.Text.Json.Serialization;

namespace shared.Models;

public class ErrorResponse
{
    public string TraceId { get; set; }
    public string Message { get; set; }
    public string ErrorCode { get; set; }
    public IDictionary<string, string[]> Errors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object AdditionalData { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string DeveloperMessage { get; set; }
}
