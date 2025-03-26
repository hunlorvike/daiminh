using System.Net;


namespace shared.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message)
        : base(message, HttpStatusCode.Unauthorized, "UNAUTHORIZED")
    {
    }

    public UnauthorizedException()
        : base("You are not authorized to perform this action", HttpStatusCode.Unauthorized, "UNAUTHORIZED")
    {
    }
}
