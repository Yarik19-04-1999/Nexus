using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Nexus.Infrastructure.Core.Extensions;
using Nexus.Infrastructure.Core.Options;
using Nexus.Infrastructure.Http.HttpHandlers;
using Nexus.Infrastructure.Http.Policies;
using Polly;

namespace Nexus.Infrastructure.Http.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddExternalServiceHttpClient<TClient, TImplementation>(
        this IServiceCollection services,
        string name,
        IConfigurationSection section,
        IAsyncPolicy<HttpResponseMessage>? retryPolicy = null)
        where TClient : class
        where TImplementation : class, TClient
    {
        services.AddExternalService(name, section);
        return services.AddHttpClientCore<TClient, TImplementation, ExternalServiceOptions>(name, retryPolicy);
    }

    public static IHttpClientBuilder AddApiKeyExternalServiceHttpClient<TClient, TImplementation>(
        this IServiceCollection services,
        string name,
        IConfigurationSection section,
        IAsyncPolicy<HttpResponseMessage>? retryPolicy = null)
        where TClient : class
        where TImplementation : class, TClient
    {
        services.AddApiKeyExternalService(name, section);
        return services.AddHttpClientCore<TClient, TImplementation, ApiKeyExternalServiceOptions>(name, retryPolicy);
    }

    private static IHttpClientBuilder AddHttpClientCore<TClient, TImplementation, TOptions>(
        this IServiceCollection services,
        string name,
        IAsyncPolicy<HttpResponseMessage>? retryPolicy)
        where TClient : class
        where TImplementation : class, TClient
        where TOptions : ExternalServiceOptions
    {
        services.TryAddTransient<LoggingDelegatingHandler>();
        services.TryAddTransient<CorrelationIdDelegatingHandler>();

        return services
            .AddHttpClient<TClient, TImplementation>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<TOptions>>().Get(name);
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = options.Timeout;
            })
            .AddHttpMessageHandler<LoggingDelegatingHandler>()
            .AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
            .AddPolicyHandler(retryPolicy ?? NexusHttpPolicies.DefaultRetryPolicy);
    }
}
