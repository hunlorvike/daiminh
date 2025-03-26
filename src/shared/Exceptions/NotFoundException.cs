using System.Net;


namespace shared.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message, HttpStatusCode.NotFound, "RESOURCE_NOT_FOUND")
    {
    }

    public NotFoundException(string entityName, object id)
        : base($"{entityName} with id {id} was not found", HttpStatusCode.NotFound, "RESOURCE_NOT_FOUND")
    {
    }
}
