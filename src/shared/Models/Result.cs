using shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace shared.Models;

public class Result<T>
{
    public T? Value { get; }
    public bool IsSuccess { get; }
    public ResultType Type { get; }
    public List<Error> Errors { get; } = [];

    public bool IsFailure => !IsSuccess;

    private Result(T? value, bool isSuccess, ResultType type, List<Error> errors)
    {
        Value = value;
        IsSuccess = isSuccess;
        Type = type;
        Errors = errors;
    }

    public static Result<T> Success(T value)
        => new(value, true, ResultType.Success, []);

    public static Result<T> Failure(string code, string message)
        => new(default, false, ResultType.Failure, [new(code, message)]);

    public static Result<T> Failure(List<Error> errors)
        => new(default, false, ResultType.Failure, errors);

    public static Result<T> Validation(List<Error> errors)
        => new(default, false, ResultType.Validation, errors);

    public static Result<T> NotFound(string code = "NotFound", string message = "The requested resource was not found.")
        => new(default, false, ResultType.NotFound, [new(code, message)]);
}