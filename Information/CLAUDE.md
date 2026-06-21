# Architecture

Information follows clean architecture: `Information.Api` / `Information.Application` / `Information.Infrastructure`.

# Error handling

Information use cases implement `ISimpleUseCase<TInput, TOutput>` and throw `DomainException` instead of returning `Result<T>`. `ExceptionResponseMiddleware` (registered in `Nexus.Api.Core`) catches it and converts it to the same `DomainErrorResponse` shape that other services produce via `this.DomainError(result)`.

This is the established pattern for this service. Do not mix in `Result<T>` use cases without migrating the whole service. There is intentionally no `InformationResultConstants`.

```csharp
// correct for Information
public class GetExchangeRatesUseCase : ISimpleUseCase<GetExchangeRatesInput, IReadOnlyList<ExchangeRate>>
{
    public async Task<IReadOnlyList<ExchangeRate>> Execute(GetExchangeRatesInput input, CancellationToken cancellationToken)
    {
        var rates = await _provider.GetRates(input.Date, cancellationToken)
            ?? throw CommonExceptions.ExternalProviderNoData();
        return rates;
    }
}
```

# External providers and caching

External data sources (`IExchangeRateProvider`, `IWeatherProvider`, `IEpicGamesProvider`) are decorated with a caching wrapper at DI registration time using Scrutor.

```csharp
services.AddScoped<IExchangeRateProvider, NbuExchangeRateProvider>();
services.Decorate<IExchangeRateProvider, CachingExchangeRateProvider>();
```

Cache options (`XxxCacheOptions`) have their own validators (`IValidateOptions<T>`). Add `CacheExpiration` to `appsettings.json` for any new provider.

# Providers

Each external service has its own options class (`XxxOptions`) with `BaseUrl`, `Timeout`, and optionally `ProviderType` for strategy selection. Options are validated via `IValidateOptions<T>` implementations.

SQL migration scripts live in `Information.Infrastructure/scripts/script.sql`.
