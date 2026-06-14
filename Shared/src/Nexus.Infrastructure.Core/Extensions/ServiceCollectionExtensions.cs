using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Core.Extensions;
using Nexus.Infrastructure.Core.Options;
using Nexus.Infrastructure.Core.Validators;

namespace Nexus.Infrastructure.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalService(
        this IServiceCollection services,
        string name,
        IConfigurationSection section)
    {
        services.ConfigureOptions<ExternalServiceOptions, ExternalServiceOptionsValidator>(name, section);
        services.PostConfigure<ExternalServiceOptions>(name, o => o.BaseUrl = o.BaseUrl?.TrimEnd('/'));
        return services;
    }

    public static IServiceCollection AddApiKeyExternalService(
        this IServiceCollection services,
        string name,
        IConfigurationSection section)
    {
        services.ConfigureOptions<ApiKeyExternalServiceOptions, ApiKeyExternalServiceOptionsValidator>(name, section);
        services.PostConfigure<ApiKeyExternalServiceOptions>(name, o => o.BaseUrl = o.BaseUrl?.TrimEnd('/'));
        return services;
    }
}
