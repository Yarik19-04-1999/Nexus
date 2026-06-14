using Information.Application.Constants;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Services;
using Information.Infrastructure.Decorators;
using Information.Infrastructure.Enums;
using Information.Infrastructure.Options;
using Information.Infrastructure.Providers.EpicGames;
using Information.Infrastructure.Providers.Nbu;
using Information.Infrastructure.Providers.OpenMeteo;
using Information.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Core.Extensions;
using Nexus.Infrastructure.Core.Options;
using Nexus.Infrastructure.Core.Validators;
using Nexus.Infrastructure.Http.Extensions;

namespace Information.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    private const string NbuName = "Nbu";
    private const string OpenMeteoName = "OpenMeteo";
    private const string EpicGamesName = "EpicGames";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();

        RegisterExternalServiceOptions(services, configuration);
        RegisterExchangeRateProvider(services, configuration);
        RegisterWeatherProvider(services, configuration);
        RegisterEpicGamesProvider(services, configuration);

        return services;
    }

    private static void RegisterExternalServiceOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<ExternalServiceOptions, ExternalServiceOptionsValidator, ExternalServiceOptionsPostConfigure>(
            NbuName, configuration.GetSection($"ExternalServices:{NbuName}"));

        services.ConfigureOptions<ExternalServiceOptions, ExternalServiceOptionsValidator, ExternalServiceOptionsPostConfigure>(
            OpenMeteoName, configuration.GetSection($"ExternalServices:{OpenMeteoName}"));

        services.ConfigureOptions<ExternalServiceOptions, ExternalServiceOptionsValidator, ExternalServiceOptionsPostConfigure>(
            EpicGamesName, configuration.GetSection($"ExternalServices:{EpicGamesName}"));
    }

    private static void RegisterExchangeRateProvider(IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredOptions<ExchangeRateOptions>(ConfigurationConstants.ExchangeRateSection);

        switch (options.ProviderType)
        {
            case ExchangeRateProviderType.Nbu:
                services.AddExternalServiceHttpClient<IExchangeRateProvider, NbuExchangeRateProvider>(NbuName);
                break;
            default:
                throw new InvalidOperationException($"Unknown exchange rate provider type: {options.ProviderType}");
        }

        services.Decorate<IExchangeRateProvider, CachingExchangeRateProvider>();
    }

    private static void RegisterWeatherProvider(IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredOptions<WeatherOptions>(ConfigurationConstants.WeatherSection);

        switch (options.ProviderType)
        {
            case WeatherProviderType.OpenMeteo:
                services.AddExternalServiceHttpClient<IWeatherProvider, OpenMeteoWeatherProvider>(OpenMeteoName);
                break;
            default:
                throw new InvalidOperationException($"Unknown weather provider type: {options.ProviderType}");
        }

        services.Decorate<IWeatherProvider, CachingWeatherProvider>();
    }

    private static void RegisterEpicGamesProvider(IServiceCollection services, IConfiguration configuration)
    {
        services.AddExternalServiceHttpClient<IEpicGamesProvider, EpicGamesProvider>(EpicGamesName);
        services.Decorate<IEpicGamesProvider, CachingEpicGamesProvider>();
    }
}
