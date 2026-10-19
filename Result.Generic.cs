using AlfaHackaton.Result.Errors;

namespace AlfaHackaton.Result;

public record Result<TOut>(TOut? Content, Error? Error = null) : Result(Error)
{
    public Result() : this(default, null)
    {
    }

    public Result(string errorText, ErrorSource source = ErrorSource.Application, ErrorType errorType = ErrorType.Undefined)
        : this(default, new Error(errorText, source, errorType))
    {
    }

    public Result(Error error) : this(default, error)
    {
    }

    public TOut? Content { get; set; } = Content;

    public static implicit operator Result<TOut>(TOut? value)
    {
        return new Result<TOut>(value);
    }
}
