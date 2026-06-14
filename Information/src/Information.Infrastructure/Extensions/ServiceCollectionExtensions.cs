using Information.Application.Constants;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Services;
using Information.Application.Models.Options;
using Information.Infrastructure.Decorators;
using Information.Infrastructure.Enums;
using Information.Infrastructure.Options;
using Information.Infrastructure.Providers.Nbu;
using Information.Infrastructure.Providers.OpenMeteo;
using Information.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Nexus.Application.Core.Extensions;

namespace Information.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddHttpClient();

        services.AddSingleton<ICacheService, CacheService>();

        var exchangeRateOptions = configuration.GetRequiredOptions<ExchangeRateOptions>(ConfigurationConstants.ExchangeRateSection);
        services.Configure<ExchangeRateOptions>(configuration.GetSection(ConfigurationConstants.ExchangeRateSection));

        switch (exchangeRateOptions.ProviderType)
        {
            case ExchangeRateProviderType.Nbu:
                services.AddScoped<NbuExchangeRateProvider>();
                services.AddScoped<IExchangeRateProvider>(sp => new CachingExchangeRateProvider(
                    sp.GetRequiredService<NbuExchangeRateProvider>(),
                    sp.GetRequiredService<ICacheService>(),
                    sp.GetRequiredService<ICacheKeyProvider>(),
                    sp.GetRequiredService<IOptions<ExchangeRateCacheOptions>>()
                ));
                break;
            default:
                throw new InvalidOperationException($"Unknown exchange rate provider type: {exchangeRateOptions.ProviderType}");
        }

        var weatherOptions = configuration.GetRequiredOptions<WeatherOptions>(ConfigurationConstants.WeatherSection);
        services.Configure<WeatherOptions>(configuration.GetSection(ConfigurationConstants.WeatherSection));

        switch (weatherOptions.ProviderType)
        {
            case WeatherProviderType.OpenMeteo:
                services.AddScoped<OpenMeteoWeatherProvider>();
                services.AddScoped<IWeatherProvider>(sp => new CachingWeatherProvider(
                    sp.GetRequiredService<OpenMeteoWeatherProvider>(),
                    sp.GetRequiredService<ICacheService>(),
                    sp.GetRequiredService<ICacheKeyProvider>(),
                    sp.GetRequiredService<IOptions<WeatherCacheOptions>>()
                ));
                break;
            default:
                throw new InvalidOperationException($"Unknown weather provider type: {weatherOptions.ProviderType}");
        }

        return services;
    }
}
