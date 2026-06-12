using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Nexus.Api.Core.Options;
using Scalar.AspNetCore;

namespace Nexus.Api.Core.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapNexus(this IEndpointRouteBuilder endpoints, NexusOptions options)
    {
        if (options.UseHealthChecks)
        {
            endpoints.MapNexusHealthChecks();
        }

        if (options.UseScalarUi)
        {
            endpoints.MapNexusScalarUi();
        }

        return endpoints;
    }

    public static IEndpointRouteBuilder MapNexusScalarUi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapOpenApi();
        endpoints.MapScalarApiReference();
        return endpoints;
    }

    public static IEndpointRouteBuilder MapNexusHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false,
        });

        endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        return endpoints;
    }
}
