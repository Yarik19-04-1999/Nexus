using Information.Application.Constants;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Services;
using Information.Infrastructure.Enums;
using Information.Infrastructure.Options;
using Information.Infrastructure.Providers.Nbu;
using Information.Infrastructure.Providers.OpenMeteo;
using Information.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Core.Extensions;

namespace Information.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddHttpClient();

        var exchangeRateOptions = configuration.GetRequiredOptions<ExchangeRateOptions>(ConfigurationConstants.ExchangeRateSection);
        services.Configure<ExchangeRateOptions>(configuration.GetSection(ConfigurationConstants.ExchangeRateSection));

        services.AddSingleton<ICacheService, CacheService>();

        switch (exchangeRateOptions.ProviderType)
        {
            case ExchangeRateProviderType.Nbu:
                services.AddScoped<IExchangeRateProvider, NbuExchangeRateProvider>();
                break;
            default:
                throw new InvalidOperationException($"Unknown exchange rate provider type: {exchangeRateOptions.ProviderType}");
        }

        var weatherOptions = configuration.GetRequiredOptions<WeatherOptions>(ConfigurationConstants.WeatherSection);
        services.Configure<WeatherOptions>(configuration.GetSection(ConfigurationConstants.WeatherSection));

        switch (weatherOptions.ProviderType)
        {
            case WeatherProviderType.OpenMeteo:
                services.AddScoped<IWeatherProvider, OpenMeteoWeatherProvider>();
                break;
            default:
                throw new InvalidOperationException($"Unknown weather provider type: {weatherOptions.ProviderType}");
        }

        return services;
    }
}
