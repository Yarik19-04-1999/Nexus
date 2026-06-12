using Information.Application.Interfaces.Providers;
using Information.Infrastructure.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Information.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();

        services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
        services.AddScoped<IEpicGamesProvider, EpicGamesProvider>();
        services.AddScoped<IWeatherProvider, WeatherProvider>();

        return services;
    }
}
