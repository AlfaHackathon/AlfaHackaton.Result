using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AlfaHackaton.Result.Errors;

[JsonConverter(typeof(StringEnumConverter))]
public enum ErrorType
{
    Undefined,
    Unauthorized,
    CorruptedInfo,
    NoRights,
    NotFound
}
