using Information.Application.Constants;
using Information.Infrastructure.DbContexts;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Repositories;
using Information.Application.Interfaces.Services;
using Information.Infrastructure.Constants;
using Information.Infrastructure.Decorators;
using Information.Infrastructure.Enums;
using Information.Infrastructure.Options;
using Information.Infrastructure.Providers.EpicGames;
using Information.Infrastructure.Providers.Nbu;
using Information.Infrastructure.Providers.OpenMeteo;
using Information.Infrastructure.Repositories;
using Information.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexus.Application.Core.Extensions;
using Nexus.Infrastructure.Core.Extensions;
using Nexus.Infrastructure.EfCore.SqlServer.Extensions;
using Nexus.Infrastructure.Http.Extensions;

namespace Information.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();

        var sqlOptions = configuration.GetSqlServerOptions();
        services.AddNexusDbContext<InformationDbContext>(sqlOptions, environment);
        services.AddScoped<ITelegramUserRepository, TelegramUserRepository>();

        RegisterExchangeRateProvider(services, configuration);
        RegisterWeatherProvider(services, configuration);
        RegisterEpicGamesProvider(services, configuration);

        return services;
    }

    private static void RegisterExchangeRateProvider(IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetRequiredOptions<ExchangeRateOptions>(ConfigurationConstants.ExchangeRateSection);

        switch (options.ProviderType)
        {
            case ExchangeRateProviderType.Nbu:
                services.AddExternalServiceHttpClient<IExchangeRateProvider, NbuExchangeRateProvider>(
                    ExternalServiceConstants.Nbu,
                    ExternalServiceConstants.Nbu);
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
                services.AddExternalServiceHttpClient<IWeatherProvider, OpenMeteoWeatherProvider>(
                    ExternalServiceConstants.OpenMeteo,
                    ExternalServiceConstants.OpenMeteo);
                break;
            default:
                throw new InvalidOperationException($"Unknown weather provider type: {options.ProviderType}");
        }

        services.Decorate<IWeatherProvider, CachingWeatherProvider>();
    }

    private static void RegisterEpicGamesProvider(IServiceCollection services, IConfiguration configuration)
    {
        services.AddExternalServiceHttpClient<IEpicGamesProvider, EpicGamesProvider>(
            ExternalServiceConstants.EpicGames,
            ExternalServiceConstants.EpicGames);

        services.Decorate<IEpicGamesProvider, CachingEpicGamesProvider>();
    }
}
