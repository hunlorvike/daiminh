namespace shared.Models;

/// <summary>
/// Exception thrown when a resource is not found. (404)
/// </summary>
/// <param name="message"></param>
public class NotFoundException(string message) : Exception(message)
{
}

/// <summary>
/// Exception thrown when a request is invalid. (400)   
/// </summary>
/// <param name="message"></param>
public class ValidationException(string message) : Exception(message)
{
}

/// <summary>
/// Exception thrown when a business rule is violated. (400 | 422)
/// </summary>
/// <param name="message"></param>
public class BusinessLogicException(string message) : Exception(message)
{
}

/// <summary>
/// Exception thrown when an internal error occurs. (500)
/// </summary>
/// <param name="message"></param>
/// <param name="innerException"></param>
public class SystemException2(string message, Exception innerException) : Exception(message, innerException)
{
}