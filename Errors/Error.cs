using System.Net;
using Newtonsoft.Json;

namespace AlfaHackaton.Result.Errors;

public class Error(string title, ErrorSource source = ErrorSource.Application, ErrorType type = ErrorType.Undefined)
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Title { get; set; } = title;
    public ErrorType Type { get; set; } = type;
    public ErrorSource Source { get; set; } = source;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public HttpStatusCode? HttpStatusCode { get; set; }

    public override string ToString()
    {
        return Title ?? string.Empty;
    }
}
