using AlfaHackaton.Result.Errors;

namespace AlfaHackaton.Result;

public record Result(Error? Error = null)
{
    public int ExternalStatusCode { get; set; }
    public Error? Error { get; set; } = Error;

    public static implicit operator Result(Error error)
    {
        return new Result(error);
    }
}
