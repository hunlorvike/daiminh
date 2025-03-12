namespace shared.Models;

/// <summary>
/// Abstract base class for API responses.  Provides a common <see cref="Success"/> property.
/// </summary>
/// <param name="success">Indicates whether the operation was successful.</param>
public abstract class BaseResponse(bool success)
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; protected init; } = success;
}

/// <summary>
/// Represents an API response for error scenarios. Contains a dictionary of errors, keyed by field name.
/// </summary>
/// <param name="errors">A dictionary of errors, where the key is the field name and the value is an array of error messages.</param>
public class ErrorResponse(Dictionary<string, string[]>? errors) : BaseResponse(false)
{
    /// <summary>
    /// Gets the dictionary of errors.  If no errors are provided, returns an empty dictionary.
    /// </summary>
    public Dictionary<string, string[]> Errors { get; } = errors ?? [];

    /// <summary>
    /// Private constructor for creating an <see cref="ErrorResponse"/> with a single error.
    /// </summary>
    /// <param name="key">The field name associated with the error.</param>
    /// <param name="message">The array of error messages.</param>
    private ErrorResponse(string key, string[] message)
        : this(new Dictionary<string, string[]> { { key, message } })
    {
    }

    /// <summary>
    /// Constructor for creating an error with one field.
    /// </summary>
    /// <param name="messages">Errors</param>
    public ErrorResponse(string[] messages)
        : this("General", messages)
    {
    }
}

/// <summary>
/// Represents a successful API response, containing data and a message.
/// </summary>
/// <typeparam name="T">The type of the data returned in the response.</typeparam>
/// <param name="data">The data returned by the API.</param>
/// <param name="message">A message associated with the response (e.g., "Operation successful").</param>
public class SuccessResponse<T>(T data, string message) : BaseResponse(true)
{
    /// <summary>
    /// Gets the data returned by the API.
    /// </summary>
    public T Data { get; } = data;

    /// <summary>
    /// Gets the message associated with the response.
    /// </summary>
    public string Message { get; } = message;
}

/// <summary>
/// Represents a successful API response for paginated data.  Includes pagination information
/// along with the data and a message.
/// </summary>
/// <typeparam name="T">The type of the data returned in the response.</typeparam>
public class PaginationResponse<T> : SuccessResponse<T>
{
    /// <summary>
    /// Gets the current page number.
    /// </summary>
    public int PageNumber { get; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Gets the total number of records.
    /// </summary>
    public int TotalRecords { get; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationResponse{T}"/> class.
    /// </summary>
    /// <param name="data">The data for the current page.</param>
    /// <param name="message">A message associated with the response.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="totalRecords">The total number of records.</param>
    public PaginationResponse(T data, string message, int pageNumber, int pageSize, int totalRecords)
        : base(data, message)
    {
        PageNumber = pageNumber > 0 ? pageNumber : 1;
        PageSize = pageSize > 0 ? pageSize : 10;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);
    }
}