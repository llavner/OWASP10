namespace OWASP.Application.Common;

public class Result<T, TCode>
    where TCode : struct, Enum
{
    public bool IsSuccess { get; init; }

    public string Error { get; init; } = string.Empty;

    public T? Value { get; init; }

    public TCode? Code { get; init; }

    public static Result<T, TCode> Success(T value, TCode code) => new()
    {
        IsSuccess = true,
        Value = value,
        Code = code,
    };

    public static Result<T, TCode> Failure(TCode code, string error) => new()
    {
        IsSuccess = false,
        Error = error,
        Code = code,
    };
}
