using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Services;
using Information.Application.Models.Options;
using Information.Infrastructure.Enums;
using Information.Infrastructure.Options;
using Information.Infrastructure.Providers.Nbu;
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

        var exchangeRateOptions = configuration.GetRequiredOptions<ExchangeRateOptions>(nameof(ExchangeRateOptions));
        services.Configure<ExchangeRateOptions>(configuration.GetSection(nameof(ExchangeRateOptions)));

        services.AddOptions<ExchangeRateCacheOptions>()
            .BindConfiguration(nameof(ExchangeRateOptions))
            .Validate(o => o.CacheExpiration > TimeSpan.Zero, $"{nameof(ExchangeRateCacheOptions.CacheExpiration)} must be greater than zero.")
            .ValidateOnStart();

        services.AddSingleton<ICacheService, CacheService>();

        services.AddScoped<NbuExchangeRateProvider>();
        services.AddScoped<IExchangeRateProvider>(sp => exchangeRateOptions.ProviderType switch
        {
            ExchangeRateProviderType.Nbu => sp.GetRequiredService<NbuExchangeRateProvider>(),
            _ => throw new InvalidOperationException($"Unknown exchange rate provider type: {exchangeRateOptions.ProviderType}"),
        });

        return services;
    }
}
