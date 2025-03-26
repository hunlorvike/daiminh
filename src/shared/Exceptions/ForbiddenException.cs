using System.Net;

namespace shared.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string message)
        : base(message, HttpStatusCode.Forbidden, "FORBIDDEN")
    {
    }

    public ForbiddenException()
        : base("You do not have permission to access this resource", HttpStatusCode.Forbidden, "FORBIDDEN")
    {
    }
}
