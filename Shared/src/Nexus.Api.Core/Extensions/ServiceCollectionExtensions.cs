using System.IO.Compression;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Api.Core.Constants;
using Nexus.Api.Core.OpenApi;
using Nexus.Api.Core.Options;
using Nexus.Application.Core.Extensions;

namespace Nexus.Api.Core.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddNexusServices(this IServiceCollection services, NexusOptions options)
    {
        var healthChecks = services
            .AddNexusApiVersioning()
            .AddNexusCorrelationId()
            .AddHealthChecks();

        options.HealthCheckCustomAction?.Invoke(healthChecks);

        if (options.UseOpenApi)
        {
            services.AddNexusOpenApi();
        }

        if (options.UseResponseCompression)
        {
            services.AddNexusResponseCompression();
        }

        if (options.HasRequestTimeout())
        {
            services.AddNexusRequestTimeouts(options.RequestTimeout);
        }

        return services;
    }

    public static IServiceCollection AddNexusCorrelationId(this IServiceCollection services)
    {
        services.AddDefaultCorrelationId(options =>
        {
            options.AddToLoggingScope = true;
            options.UpdateTraceIdentifier = true;
        });

        return services;
    }

    public static IServiceCollection AddNexusOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddOperationTransformer<CommonErrorResponsesOperationTransformer>();
        });
        return services;
    }

    public static IServiceCollection AddNexusApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static IHealthChecksBuilder AddNexusHealthChecks(this IServiceCollection services)
        => services.AddHealthChecks();

    public static IServiceCollection AddNexusResponseCompression(this IServiceCollection services)
    {
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
            options.Level = CompressionLevel.Fastest);

        services.Configure<GzipCompressionProviderOptions>(options =>
            options.Level = CompressionLevel.Optimal);

        return services;
    }

    public static IServiceCollection AddNexusRequestTimeouts(this IServiceCollection services, TimeSpan? defaultRequestTimeout = null)
    {
        if (defaultRequestTimeout <= TimeSpan.Zero)
        {
            throw new ArgumentException(ValidationErrorMessages.RequestTimeoutShouldBeGreaterThenZero, nameof(defaultRequestTimeout));
        }

        services.AddRequestTimeouts(options =>
        {
            options.DefaultPolicy = new RequestTimeoutPolicy
            {
                Timeout = defaultRequestTimeout ?? CommonConstants.DefaultRequestTimeout,
            };
        });

        return services;
    }

    public static IServiceCollection AddNexusCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetRequiredOptions<string[]>(ConfigSectionConstants.AllowedOrigins);

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }
}
