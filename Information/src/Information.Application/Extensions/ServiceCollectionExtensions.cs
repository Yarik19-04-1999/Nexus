using FluentValidation;
using Information.Application.Constants;
using Information.Application.Interfaces.Services;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Options;
using Information.Application.Services;
using Information.Application.UseCases;
using Information.Application.Validators;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Core.Extensions;

namespace Information.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddOptions<ExchangeRateCacheOptions>()
            .BindConfiguration(ConfigurationConstants.ExchangeRateSection)
            .WithValidator<ExchangeRateCacheOptions, ExchangeRateCacheOptionsValidator>()
            .ValidateOnStart();

        services.AddOptions<WeatherCacheOptions>()
            .BindConfiguration(ConfigurationConstants.WeatherSection)
            .WithValidator<WeatherCacheOptions, WeatherCacheOptionsValidator>()
            .ValidateOnStart();

        services.AddOptions<EpicGamesCacheOptions>()
            .BindConfiguration(ConfigurationConstants.EpicGamesSection)
            .WithValidator<EpicGamesCacheOptions, EpicGamesCacheOptionsValidator>()
            .ValidateOnStart();

        services.AddSingleton<ICacheKeyProvider, CacheKeyProvider>();

        services.AddScoped<IGetExchangeRatesUseCase, GetExchangeRatesUseCase>();
        services.AddScoped<IGetExchangeRateHistoryUseCase, GetExchangeRateHistoryUseCase>();

        services.AddScoped<IGetHourlyWeatherUseCase, GetHourlyWeatherUseCase>();
        services.AddScoped<IGetDailyWeatherUseCase, GetDailyWeatherUseCase>();

        services.AddScoped<IGetEpicFreeGamesUseCase, GetEpicFreeGamesUseCase>();

        services.AddScoped<ISetUserLanguageUseCase, SetUserLanguageUseCase>();
        services.AddScoped<IGetUserLanguageUseCase, GetUserLanguageUseCase>();

        // services.AddValidatorsFromAssemblyContaining<SetUserLanguageInputValidator>();

        return services;
    }
}
