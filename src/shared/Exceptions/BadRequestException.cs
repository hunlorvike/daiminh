using System.Net;

namespace shared.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException(string message)
        : base(message, HttpStatusCode.BadRequest, "BAD_REQUEST")
    {
    }

    public BadRequestException(string message, object additionalData)
        : base(message, HttpStatusCode.BadRequest, "BAD_REQUEST", additionalData)
    {
    }
}