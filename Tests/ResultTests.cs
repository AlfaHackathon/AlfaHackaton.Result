using System.Net;
using AlfaHackaton.Result.Errors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AlfaHackaton.Result.Tests;

public class ResultTests
{
    [Fact]
    public void ImplicitConversion_ReturnsOriginalContent()
    {
        // Сервис возвращает значение без ручной упаковки в Result<T>.
        var content = new Payload { Name = "Хакатон" };

        Result<Payload> result = content;

        Assert.Same(content, result.Content);
        Assert.Null(result.Error);
    }

    [Fact]
    public void ErrorConstructor_PreservesErrorAndHttpStatus()
    {
        // Контракт external-сервиса: чужая ошибка передаётся дальше без потери исходного статуса.
        var error = new Error("Не удалось получить товар", ErrorSource.External, ErrorType.NotFound)
        {
            HttpStatusCode = HttpStatusCode.NotFound
        };

        var typed = new Result<Payload>(error);
        Result untyped = error;

        Assert.Same(error, typed.Error);
        Assert.Same(error, untyped.Error);
        Assert.Null(typed.Content);
        Assert.NotNull(typed.Error);
        Assert.Equal(HttpStatusCode.NotFound, typed.Error.HttpStatusCode);
    }

    [Fact]
    public void ErrorTextConstructor_PreservesSourceAndType()
    {
        var result = new Result<Payload>("Нет доступа", ErrorSource.Client, ErrorType.NoRights);

        Assert.NotNull(result.Error);
        Assert.Equal("Нет доступа", result.Error.Title);
        Assert.Equal(ErrorSource.Client, result.Error.Source);
        Assert.Equal(ErrorType.NoRights, result.Error.Type);
        Assert.Null(result.Content);
    }

    [Fact]
    public void JsonRoundTrip_PreservesContentAndError()
    {
        // Контракт переноса: Result остаётся сериализуемой моделью ответа, а не транспортным типом.
        var original = new Result<Payload>(new Payload { Name = "Хакатон" }, new Error("Ошибка", ErrorSource.External, ErrorType.NotFound))
        {
            ExternalStatusCode = 404
        };

        var json = JsonConvert.SerializeObject(original);
        var restored = JsonConvert.DeserializeObject<Result<Payload>>(json);

        Assert.NotNull(restored);
        Assert.Equal("Хакатон", restored.Content?.Name);
        Assert.Equal("Ошибка", restored.Error?.Title);
        Assert.Equal(ErrorType.NotFound, restored.Error?.Type);
        Assert.Equal(404, restored.ExternalStatusCode);
        Assert.Contains("\"NotFound\"", json);
    }

    [Fact]
    public void JsonError_OmitsAbsentTitleAndStatus()
    {
        var error = new Error("Ошибка") { Title = null };

        var json = JObject.Parse(JsonConvert.SerializeObject(error));

        Assert.Null(json.Property(nameof(Error.Title)));
        Assert.Null(json.Property(nameof(Error.HttpStatusCode)));
    }

    private sealed class Payload
    {
        public string Name { get; set; } = string.Empty;
    }
}
