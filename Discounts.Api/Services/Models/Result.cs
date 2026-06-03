namespace Discounts.Api.Services.Models;

public class Result<T>
{
    public Result(string error)
    {
        Error = error;
    }

    public Result(T value)
    {
        Value = value;
    }

    public string Error { get; }
    public T Value { get; }
}
