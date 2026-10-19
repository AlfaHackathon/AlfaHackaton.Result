# AlfaHackaton.Result

Результаты операций и ошибки для .NET 10.

## Вернуть результат

```csharp
using AlfaHackaton.Result;
using AlfaHackaton.Result.Errors;

public Result<Item> GetItem(int id)
{
    if (id <= 0)
    {
        return new("Некорректный идентификатор", ErrorSource.Client, ErrorType.CorruptedInfo);
    }

    return new Item { Id = id };
}
```

Значение неявно преобразуется в `Result<T>`. Успешный результат содержит `Content` и пустой `Error`.

## Передать ошибку

```csharp
var error = new Error("Не удалось получить товар", ErrorSource.External, ErrorType.NotFound)
{
    HttpStatusCode = System.Net.HttpStatusCode.NotFound
};

var result = new Result<Item>(error);
```

Для операции без содержимого используется `Result`. В `Error` доступны `Title`, `Type`, `Source`
и необязательный `HttpStatusCode`. `Result<T>` допускает пустой `Content`; наличие ошибки определяется
полем `Error`, а не содержимым результата.

## Проверить результат

```csharp
var result = service.GetItem(id);
if (result.Error is not null)
{
    return new Result<Item>(result.Error);
}

var item = result.Content;
```

## Проверка

```powershell
dotnet test AlfaHackaton.Result.slnx
```

Сборка NuGet-пакета:

```powershell
dotnet pack AlfaHackaton.Result.csproj --configuration Release --output artifacts/nupkg
```
