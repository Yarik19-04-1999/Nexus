using Information.Application.Interfaces.UseCases;
using Information.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Information.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGetExchangeRatesUseCase, GetExchangeRatesUseCase>();
        services.AddScoped<IGetEpicGiveawaysUseCase, GetEpicGiveawaysUseCase>();
        services.AddScoped<IGetWeatherUseCase, GetWeatherUseCase>();

        return services;
    }
}
