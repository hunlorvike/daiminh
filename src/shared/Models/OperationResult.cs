namespace shared.Models;
public class OperationResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public static OperationResult SuccessResult(string? message = null) => new OperationResult { Success = true, Message = message };
    public static OperationResult FailureResult(string? message = null, List<string>? errors = null) => new OperationResult { Success = false, Message = message, Errors = errors ?? new List<string>() };
}

public class OperationResult<T> : OperationResult
{
    public T? Data { get; set; }
    public static OperationResult<T> SuccessResult(T data, string? message = null) => new OperationResult<T> { Success = true, Data = data, Message = message };
    public new static OperationResult<T> FailureResult(string? message = null, List<string>? errors = null) => new OperationResult<T> { Success = false, Message = message, Errors = errors ?? new List<string>() };
}