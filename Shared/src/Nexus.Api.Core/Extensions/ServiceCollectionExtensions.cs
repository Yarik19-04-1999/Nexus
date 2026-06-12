using System.IO.Compression;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Api.Core.Constants;
using Microsoft.AspNetCore.Http.Timeouts;
using Nexus.Application.Core.Extensions;

namespace Nexus.Api.Core.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddNexusCorrelationId(this IServiceCollection services)
    {
        services.AddCorrelationId(options =>
        {
            options.AddToLoggingScope = true;
            options.UpdateTraceIdentifier = true;
        });

        return services;
    }

    public static IServiceCollection AddNexusOpenApi(this IServiceCollection services)
        => services.AddOpenApi();

    public static IServiceCollection AddNexusApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning();
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
        services.AddRequestTimeouts(options =>
        {
            options.DefaultPolicy = new RequestTimeoutPolicy
            {
                Timeout = defaultRequestTimeout ?? RequestTimeoutConstants.Default,
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
