namespace Core.Common.Models;

public abstract class BaseResponse(bool success)
{
    public bool Success { get; protected init; } = success;
}

public class ErrorResponse(Dictionary<string, string>? errors) : BaseResponse(false)
{
    public Dictionary<string, string> Errors { get; } = errors ?? new Dictionary<string, string>();

    private ErrorResponse(string key, string message)
        : this(new Dictionary<string, string> { { key, message } })
    {
    }

    public ErrorResponse(string message)
        : this("General", message)
    {
    }
}

public class SuccessResponse<T>(T data, string message) : BaseResponse(true)
{
    public T Data { get; } = data;
    public string Message { get; } = message;
}

public class PaginationResponse<T> : SuccessResponse<T>
{
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalRecords { get; }
    public int TotalPages { get; }

    public PaginationResponse(T data, string message, int pageNumber, int pageSize, int totalRecords)
        : base(data, message)
    {
        PageNumber = pageNumber > 0 ? pageNumber : 1;
        PageSize = pageSize > 0 ? pageSize : 10;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
    }
}