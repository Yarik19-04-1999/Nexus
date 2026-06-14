using Information.Application.Constants;
using Information.Application.Interfaces.Services;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Options;
using Information.Application.Services;
using Information.Application.UseCases;
using Information.Application.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Information.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ExchangeRateCacheOptions>(configuration.GetSection(ConfigurationConstants.ExchangeRateSection));
        services.AddSingleton<IValidateOptions<ExchangeRateCacheOptions>, ExchangeRateCacheOptionsValidator>();
        services.AddOptions<ExchangeRateCacheOptions>().ValidateOnStart();

        services.Configure<WeatherCacheOptions>(configuration.GetSection(ConfigurationConstants.WeatherSection));
        services.AddSingleton<IValidateOptions<WeatherCacheOptions>, WeatherCacheOptionsValidator>();
        services.AddOptions<WeatherCacheOptions>().ValidateOnStart();

        services.AddSingleton<ICacheKeyProvider, CacheKeyProvider>();

        services.AddScoped<IGetExchangeRatesUseCase, GetExchangeRatesUseCase>();
        services.AddScoped<IGetExchangeRateHistoryUseCase, GetExchangeRateHistoryUseCase>();

        services.AddScoped<IGetHourlyWeatherUseCase, GetHourlyWeatherUseCase>();
        services.AddScoped<IGetDailyWeatherUseCase, GetDailyWeatherUseCase>();

        return services;
    }
}
