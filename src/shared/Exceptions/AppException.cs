using System.Net;

namespace shared.Exceptions;

public class AppException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string ErrorCode { get; }
    public object AdditionalData { get; }

    public AppException(string message)
        : base(message)
    {
        StatusCode = HttpStatusCode.InternalServerError;
        ErrorCode = "INTERNAL_ERROR";
    }

    public AppException(string message, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = HttpStatusCode.InternalServerError;
        ErrorCode = "INTERNAL_ERROR";
    }

    public AppException(string message, HttpStatusCode statusCode)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = "INTERNAL_ERROR";
    }

    public AppException(string message, string errorCode)
        : base(message)
    {
        StatusCode = HttpStatusCode.InternalServerError;
        ErrorCode = errorCode;
    }

    public AppException(string message, HttpStatusCode statusCode, string errorCode)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public AppException(string message, HttpStatusCode statusCode, string errorCode, object additionalData)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        AdditionalData = additionalData;
    }
}
