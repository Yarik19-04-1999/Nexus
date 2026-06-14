using CorrelationId.HttpClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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
        IAsyncPolicy<HttpResponseMessage>? retryPolicy = null)
        where TClient : class
        where TImplementation : class, TClient
        => services.AddExternalServiceHttpClient<TClient, TImplementation, ExternalServiceOptions>(name, retryPolicy);

    public static IHttpClientBuilder AddExternalServiceHttpClient<TClient, TImplementation, TOptions>(
        this IServiceCollection services,
        string name,
        IAsyncPolicy<HttpResponseMessage>? retryPolicy = null)
        where TClient : class
        where TImplementation : class, TClient
        where TOptions : ExternalServiceOptions
    {
        services.TryAddTransient<LoggingDelegatingHandler>();

        return services
            .AddHttpClient<TClient, TImplementation>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<TOptions>>().Get(name);
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = options.Timeout;
            })
            .AddHttpMessageHandler<LoggingDelegatingHandler>()
            .AddCorrelationIdForwarding()
            .AddPolicyHandler(retryPolicy ?? NexusHttpPolicies.DefaultRetryPolicy);
    }
}
