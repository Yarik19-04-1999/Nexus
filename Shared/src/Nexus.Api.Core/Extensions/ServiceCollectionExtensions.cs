using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Api.Core.Constants;
using Nexus.Api.Core.CorrelationId;
using Nexus.Application.Core.Extensions;

namespace Nexus.Api.Core.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddNexusCorrelationId(this IServiceCollection services)
    {
        services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();
        return services;
    }

    public static IServiceCollection AddNexusApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning();
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
